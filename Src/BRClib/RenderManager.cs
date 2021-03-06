﻿// Part of the Blender Render Controller project
// https://github.com/rehdi93/BlenderRenderController
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
using static BRClib.Global;

namespace BRClib
{
    public class RenderManager
    {
        public bool InProgress => _RunWorker;

        public bool WasAborted
        {
            get => _Aborted;
            private set => _Aborted = value;
        }

        public event EventHandler<RenderProgressInfo> ProgressChanged;
        public event EventHandler<AfterRenderAction> AfterRenderStarted;
        public event EventHandler<BrcRenderResult> Finished;


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

            _RunWorker = true;
            _Worker = new Thread(QLoop);
            _Worker.Name = "Render queue thread";
            _Worker.Start();
        }

        public void Abort()
        {
            if (InProgress)
            {
                WasAborted = true;
                _RunWorker = false;

                DisposeProcesses();

                _Worker.Join();
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
            
            WasAborted = false;
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

        void QLoop()
        {
            while (_RunWorker)
            {
                TryQueueRenderProcess();
                ReportProgress(_FramesRendered.Count, _State.ChunksCompleted);
                Thread.Sleep(250);
            }

            if (WasAborted) return;

            OnChunksRenderFinished();
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

        private void ReportProgress(int framesRendered, int chunksCompleted)
        {
            if (_reportCount++ % 3 == 0)
            {
                ProgressChanged?.Invoke(this, new RenderProgressInfo(framesRendered, chunksCompleted));
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

        // decrement counts when a process exits, stops the worker loop when the 
        // 'ToDo' count reaches 0
        private void Render_Exited(object sender, EventArgs e)
        {
            _State.ChunksInProgress--;

            logger.Trace("Render proc exited with code {0}", (sender as Process).ExitCode);

            if (Interlocked.Decrement(ref _State.ChunksToDo) == 0)
            {
                _RunWorker = false;

                Debug.Assert(_FramesRendered.ToList().Count == _Job.Chunks.TotalLength(),
                            "Frames counted don't match the ChunkList TotalLenght");
            }
        }

        private void Render_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                const string label = "Fra:";
                if (e.Data.IndexOf(label, StringComparison.InvariantCulture) == 0)
                {
                    var frame = e.Data.Split(' ')[0].Replace(label, "");
                    _FramesRendered.Add(int.Parse(frame));
                }
            }
        }

        private void OnChunksRenderFinished()
        {
            bool renderOk = _Processes.TrueForAll(p => p.ExitCode == 0);
            _Processes.Clear();

            if (!renderOk)
            {
                logger.Error("One or more render processes did not complete sucessfully");
				Finished?.Invoke(this, BrcRenderResult.ChunkRenderFailed);
                return;
            }

            logger.Info("RENDER FINISHED");

            // Send a '100%' ProgressReport
            ReportProgress(_FramesRendered.Count, _State.TotalChunks);

            AfterRenderStarted?.Invoke(this, Settings.AfterRender);

            BrcRenderResult result = AfterRenderProc(Settings.AfterRender);

            Finished?.Invoke(this, result);
        }

        private BrcRenderResult AfterRenderProc(AfterRenderAction action)
        {
            if (WasAborted) return BrcRenderResult.Aborted;

            if (action == AfterRenderAction.NOTHING) 
                return BrcRenderResult.Ok;

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

            _Processes.Add(mixdownProc);
            _Processes.Add(concatProc);

            var arReports = new Dictionary<string, ProcessResult>
            {
                ["mixdown"] = null,
                ["concat"] = null
            };

            if (WasAborted) return BrcRenderResult.Aborted;

            switch (action)
            {
                case AfterRenderAction.JOIN | AfterRenderAction.MIXDOWN:

                    mixdownProc = mixdowncmd.GetProcess();
                    RunProc(ref mixdownProc, "mixdown");

                    if (WasAborted) return BrcRenderResult.Aborted;

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

            if (WasAborted) return BrcRenderResult.Aborted;

            // check for bad exit codes
            if (mixdownProc?.ExitCode != 0 || concatProc?.ExitCode != 0)
            {
                string arReportFile = Path.Combine(_Job.OutputPath, "AfterRenderReport_" + 
                                        Path.ChangeExtension(Path.GetRandomFileName(), "txt"));

                using (var sw = File.AppendText(arReportFile))
                {
                    foreach (var item in arReports)
                    {
                        if (WasAborted)
                            break;
                        if (item.Value.ExitCode == 0)
                            continue;

                        sw.Write("\n\n");
                        sw.Write(item.Key);
                        sw.WriteLine("Exit code: {0}\n\nStd Error:\n{1}\n\n\nStd Output:\n{2}",
                                    item.Value.ExitCode,
                                    item.Value.StdError,
                                    item.Value.StdOutput);
                    }
                }

                if (mixdowncmd.ExitCode != 0)
                {
                    return BrcRenderResult.MixdownFail;
                }
                else if (concatcmd.ExitCode != 0)
                {
                    return BrcRenderResult.ConcatFail;
                }
                else
                {
                    return BrcRenderResult.Unexpected;
                }
            }
            else
            {
                return WasAborted ? BrcRenderResult.Aborted : BrcRenderResult.Ok;
            }


            void RunProc(ref Process proc, string key)
            {
                proc.Start();

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

        }


        List<Process> _Processes;
        FrameSet _FramesRendered;
        RenderJob _Job;
        byte _reportCount;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        RenderState _State;

        volatile bool _RunWorker, _Aborted;
        Thread _Worker;


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
