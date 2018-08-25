// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BRClib.Commands;
using BRClib.Extentions;
using System.Threading;
using System.IO;
using System.Diagnostics;
using FrameSet = System.Collections.Concurrent.ConcurrentHashSet<int>;
using Timer = System.Timers.Timer;
using static BRClib.Global;

namespace BRClib
{
    public class RenderManager
    {
        List<Process> _Processes;
        FrameSet _FramesRendered;
        RenderJob _Job;
        CancellationTokenSource _arCts;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        struct RenderState
        {
            public int ChunksToDo,
                       TotalChunks, 
                       ChunksInProgress,
                       Index, 
                       MaxCores;

            public bool CanQueue =>
                Index < TotalChunks && ChunksInProgress < MaxCores;

            public int ChunksCompleted => TotalChunks - ChunksToDo;

        } RenderState _State;

        Timer _Timer;
        readonly object _syncLock = new object();


        public RenderManager()
        {
            _Timer = new Timer
            {
                Interval = 250,
                AutoReset = true
            };

            _Timer.Elapsed += delegate
            {
                lock (_syncLock)
                {
                    TryQueueRenderProcess();
                    ReportProgress(_FramesRendered.Count, _State.ChunksCompleted);
                }
            };
        }

        string MixdownFile
        {
            get
            {
                var mixdownFmt = _Job.AudioCodec;
                var projName = _Job.ProjectName;

                if (projName == null) return null;

                switch (mixdownFmt)
                {
                    case "PCM":
                        return Path.ChangeExtension(projName, "wav");
                    case "VORBIS":
                        return Path.ChangeExtension(projName, "ogg");
                    case null:
                    case "NONE":
                        return Path.ChangeExtension(projName, "ac3");
                    default:
                        return Path.ChangeExtension(projName, mixdownFmt.ToLower());
                }
            }
        }


        public bool InProgress => _Timer.Enabled;

        public bool WasAborted { get; private set; }

        public AfterRenderAction Action => Settings.AfterRender;

        public Renderer Renderer => Settings.Renderer;

        public event EventHandler<RenderProgressInfo> ProgressChanged;
        public event EventHandler<AfterRenderAction> AfterRenderStarted;
        public event EventHandler<BrcRenderResult> Finished;

        public void Abort()
        {
            if (InProgress)
            {
                _Timer.Stop();
                WasAborted = true;
                _arCts.Cancel();
                DisposeProcesses();
                logger.Warn("RENDER ABORTED");

                Finished?.Invoke(this, BrcRenderResult.Aborted);
            }
        }

        public void Setup(RenderJob job)
        {
            if (InProgress)
            {
                Abort();
                throw new InvalidOperationException("Cannot change settings while a render is in progress!");
            }

            _Job = job;
        }

        public void StartAsync()
        {
            if (InProgress)
            {
                Abort();
                throw new InvalidOperationException("A render is already in progress");
            }

            CheckForValidProperties();

            ResetFields();

            logger.Info("RENDER STARTING");
            _Timer.Start();
        }

        private void CheckForValidProperties()
        {
            string[] mustHaveValues = {
                Global.Settings.BlenderProgram,
                Global.Settings.FFmpegProgram
            };

            if (mustHaveValues.Any(x => string.IsNullOrWhiteSpace(x)))
            {
                throw new Exception("Required programs missing");
            }

            if (_Job.Chunks.Count == 0)
            {
                throw new Exception("Chunk list is empty");
            }

            if (!File.Exists(_Job.BlendFile))
            {
                throw new FileNotFoundException("Could not find 'blend' file", _Job.BlendFile);
            }

            if (!Directory.Exists(_Job.ChunksDir))
            {
                try
                {
                    Directory.CreateDirectory(_Job.ChunksDir);
                }
                catch (Exception inner)
                {
                    throw new Exception("Could not create 'chunks' folder", inner);
                }
            }
        }

        void ResetFields()
        {
            _Processes = _Job.Chunks.Select(CreateRenderProcess).ToList();

            _FramesRendered = new FrameSet();

            _State = new RenderState
            {
                TotalChunks = _Job.Chunks.Count,
                ChunksToDo = _Job.Chunks.Count,
                MaxCores = _Job.MaxCores
            };
            
            _arCts = new CancellationTokenSource();
            WasAborted = false;
        }

        /// <summary>
        /// Analizes the files in the specified folder and returns
        /// a list of valid chunks, ordered by frame-range
        /// </summary>
        /// <param name="chunkFolderPath"></param>
        /// <returns></returns>
        public static List<string> GetChunkFiles(string chunkFolderPath)
        {
            var dirFiles = Directory.EnumerateFiles(chunkFolderPath, "*.*",
                                            SearchOption.TopDirectoryOnly);

            string[] validExts = RenderFormats.VideoFileExts;

            var orderedChunks = dirFiles
                .Select(f => new
                {
                    file = f,
                    split = f.Split('-')
                })
                .Where(t => t.split.Length > 2 && validExts.Contains(Path.GetExtension(t.file)))
                .Select(t => new
                {
                    frame = int.TryParse(t.split[t.split.Length-2], out int x) ? x : -1,
                    t.file
                })
                .TakeWhile(t => t.frame != -1)
                .OrderBy(t => t.frame)
                .Select(t => t.file)
                ;

            return orderedChunks.ToList();
        }

        static string CreateConcatFile(List<string> chunkFilePaths, string concatDir)
        {
            var concatFile = Path.Combine(concatDir, "chunklist.txt");

            using (var sw = File.CreateText(concatFile))
            {
                foreach (var filePath in chunkFilePaths)
                {
                    sw.WriteLine("file '{0}'", filePath);
                }
            }

            return concatFile;
        }

        private void TryQueueRenderProcess()
        {
            if (_State.CanQueue)
            {
                var proc = _Processes[_State.Index];
                proc.Start();
                proc.BeginOutputReadLine();

                logger.Trace("Started render proc {0}, frames: {1}", _State.Index, _Job.Chunks[_State.Index]);

                _State.ChunksInProgress++;
                _State.Index++;
            }
        }

        byte _reportCount;
        private void ReportProgress(int framesRendered, int chunksCompleted)
        {
            if (_reportCount++ % 4 == 0)
            {
                ProgressChanged?.Raise(this, new RenderProgressInfo(framesRendered, chunksCompleted));
                _reportCount = 0;
            }
        }

        Process CreateRenderProcess(Chunk chunk)
        {
            var render = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Settings.BlenderProgram,
                    Arguments = string.Format(Settings.ArgFormats["render"], 
                        _Job.BlendFile, 
                        Path.Combine(_Job.ChunksDir, _Job.ProjectName + "-#"), 
                        Settings.Renderer, chunk.Start, chunk.End),

                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                },
                EnableRaisingEvents = true
            };

            render.OutputDataReceived += Render_OutputDataReceived;
            render.Exited += Render_Exited;

            return render;
        }

        // decrement counts when a process exits, stops the timer when the 
        // 'ToDo' count reaches 0
        private void Render_Exited(object sender, EventArgs e)
        {
            _State.ChunksInProgress--;

            logger.Trace("Render proc exited with code {0}", (sender as Process).ExitCode);

            if (Interlocked.Decrement(ref _State.ChunksToDo) == 0)
            {
                _Timer.Stop();

                Debug.Assert(_FramesRendered.ToList().Count == _Job.Chunks.TotalLength(),
                            "Frames counted don't match the ChunkList TotalLenght");

                OnChunksRenderFinished();
            }
        }

        private void Render_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                if (e.Data.IndexOf("Fra:", StringComparison.InvariantCulture) == 0)
                {
                    var line = e.Data.Split(' ')[0].Replace("Fra:", "");
                    _FramesRendered.Add(int.Parse(line));
                }
            }
        }

        private void OnChunksRenderFinished()
        {
            bool renderOk = _Processes.TrueForAll(p => p.ExitCode == 0);
            _Processes.Clear();

            if (!renderOk)
            {
                Finished?.Raise(this, BrcRenderResult.ChunkRenderFailed);
                logger.Error("One or more render processes did not complete sucessfully");
                return;
            }

            logger.Info("RENDER FINISHED");

            // Send a '100%' ProgressReport
            ReportProgress(_FramesRendered.Count, _State.TotalChunks);

            AfterRenderStarted?.Raise(this, Action);

            Task.Factory.StartNew(AfterRenderProc, Action, _arCts.Token)
            .ContinueWith(t =>
            {
                BrcRenderResult result;
                if (!t.Result)
                {
                    if (WasAborted)
                        result = BrcRenderResult.Aborted;
                    else
                        result = BrcRenderResult.AfterRenderFailed;
                }
                else
                {
                    result = BrcRenderResult.AllOk;
                }

                return result;
            })
            .ContinueWith(t => Finished?.Raise(this, t.Result));
        }


        private bool AfterRenderProc(object state)
        {
            var action = (AfterRenderAction)state;

            if (action == AfterRenderAction.NOTHING)
            {
                return true;
            }

            logger.Info("AfterRender started. Action: {0}", action);

            var chunkFiles = GetChunkFiles(_Job.ChunksDir);
            string concatFile = null;

            if (action.HasFlag(AfterRenderAction.JOIN))
            {
                if (chunkFiles.Count == 0)
                {
                    throw new Exception("Failed to query chunk files");
                }

                concatFile = CreateConcatFile(chunkFiles, _Job.ChunksDir);

                Debug.Assert(File.Exists(concatFile),
                    "concatFile was not created, but chunkFiles is not empty");
            }

            var videoExt = Path.GetExtension(chunkFiles.First());
            var projFinalPath = Path.Combine(_Job.OutputPath, _Job.ProjectName + videoExt);
            var mixdownPath = Path.Combine(_Job.OutputPath, MixdownFile);

            var mixdowncmd = new MixdownCmd()
            {
                BlendFile = _Job.BlendFile,
                Range = _Job.Chunks.GetFullRange(),
                OutputFolder = _Job.OutputPath
            };

            var concatcmd = new ConcatCmd()
            {
                ConcatTextFile = concatFile,
                OutputFile = projFinalPath,
                Duration = _Job.Duration,
                MixdownFile = mixdownPath
            };

            Process mixdownProc = null, concatProc = null;

            var arReports = new Dictionary<string, ProcessResult>
            {
                ["mixdown"] = null,
                ["concat"] = null
            };

            if (_arCts.IsCancellationRequested) return false;

            switch (action)
            {
                case AfterRenderAction.JOIN | AfterRenderAction.MIXDOWN:

                    mixdownProc = mixdowncmd.GetProcess();
                    RunProc(ref mixdownProc, "mixdown");

                    if (_arCts.IsCancellationRequested) return false;

                    concatProc = concatcmd.GetProcess();
                    RunProc(ref concatProc, "concat");

                    break;
                case AfterRenderAction.JOIN:

                    // null out MixdownFile so it generates the proper Args
                    concatcmd.MixdownFile = null;

                    concatProc = concatcmd.GetProcess();
                    RunProc(ref concatProc, "concat");

                    break;
                case AfterRenderAction.MIXDOWN:

                    mixdownProc = mixdowncmd.GetProcess();
                    RunProc(ref mixdownProc, "mixdown");

                    break;
                default:
                    break;
            }


            // check for bad exit codes
            var badProcResults = _Processes.Where(p => p != null && p.ExitCode != 0).ToArray();

            if (badProcResults.Length > 0)
            {
                string arReportFile = Path.Combine(_Job.OutputPath, "AfterRenderReport_" + 
                                        Path.ChangeExtension(Path.GetRandomFileName(), "txt"));

                using (var sw = File.AppendText(arReportFile))
                {
                    // do not write reports if exit code was caused by cancellation
                    if (!_arCts.IsCancellationRequested)
                    {
                        if (mixdownProc?.ExitCode != 0)
                        {
                            WriteReport(sw, "Mixdown ", arReports["mixdown"]);
                        }

                        if (concatProc?.ExitCode != 0)
                        {
                            WriteReport(sw, "FFMpeg concat ", arReports["concat"]);
                        }
                    }

                }
                return false;
            }
            else
            {
                return !_arCts.IsCancellationRequested;
            }


            void WriteReport(StreamWriter writer, string title, ProcessResult result)
            {
                writer.Write("\n\n");
                writer.Write(title);
                writer.WriteLine(string.Format("Exit code: {0}\n\nStd Error:\n{1}\n\n\nStd Output:\n{2}",
                                    result.ExitCode,
                                    result.StdError,
                                    result.StdOutput));
            }

            void RunProc(ref Process proc, string key)
            {
                proc.Start();
                _Processes.Add(proc);

                var soTask = proc.StandardOutput.ReadToEndAsync();
                var seTask = proc.StandardError.ReadToEndAsync();

                proc.WaitForExit();

                arReports[key] = new ProcessResult(proc.ExitCode, soTask.Result, seTask.Result);
            }

        }


        private void DisposeProcesses()
        {
            var procList = _Processes.ToList();

            foreach (var p in procList)
            {
                try
                {
                    p.Exited -= Render_Exited;
                    p.OutputDataReceived -= Render_OutputDataReceived;

                    if (!p.HasExited)
                    {
                        p.Kill();
                    }
                }
                catch (Exception ex)
                {
                    // Processes may be in an invalid state, just swallow the errors 
                    // since we're diposing them anyway
                    Debug.WriteLine(ex.Message, "RenderManager Proc dispose");
                }
                finally
                {
                    p.Dispose();
                }
            }

        }

    }

    public class RenderProgressInfo : EventArgs
    {
        public int FramesRendered { get; }
        public int PartsCompleted { get; }

        public RenderProgressInfo(int framesRendered, int partsCompleted)
        {
            FramesRendered = framesRendered;
            PartsCompleted = partsCompleted;
        }
    }

    public struct RenderJob
    {
        public string BlendFile, AudioCodec, ProjectName, ChunksDir, OutputPath;
        public TimeSpan Duration;
        public int MaxCores;

        public IReadOnlyList<Chunk> Chunks;
    }

}
