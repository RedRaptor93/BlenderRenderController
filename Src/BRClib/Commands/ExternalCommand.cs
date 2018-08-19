// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Threading;
using System.Threading.Tasks;
using BRClib.Extentions;
using NLog;

namespace BRClib.Commands
{
    /// <summary>
    /// Base class that represents a external process / command
    /// </summary>
    public abstract class ExternalCommand
    {
        public ExternalCommand(string programPath)
        {
            ProgramPath = programPath;
            Log = LogManager.GetLogger(GetType().FullName);
            _procName = Path.GetFileNameWithoutExtension(ProgramPath);
        }


        public string ProgramPath
        {
            get => _progPath;
            set
            {
                // account for relative paths
                _progPath = Path.GetFullPath(value);
            }
        }
        public string StdOutput { get; protected set; }
        public string StdError { get; protected set; }
        public int ExitCode { get; protected set; }

        protected Logger Log { get; private set; }

        protected virtual string ReportFileName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_rfn))
                {
                    var tmp = Path.GetRandomFileName();
                    _rfn = _procName + '_' + Path.ChangeExtension(tmp, "txt");
                }

                return _rfn;
            }
        }

        public Process GetProcess()
        {
            return CreateProcess();
        }

        public virtual Task<int> RunAsync(CancellationToken token = default)
        {
            return RunProcAsync(GetProcess(), token);
        }

        public virtual void SaveReport(string folderPath)
        {
            var filePath = Path.Combine(folderPath, ReportFileName);

            using (var sw = File.AppendText(filePath))
            {
                sw.WriteLine(GenerateReport());
                sw.Write("\n\n");
            }
        }

        public string GenerateReport()
        {
            return string.Format(REPORT_FMT, _procName, ExitCode, StdError, StdOutput);
        }

        public override string ToString()
        {
            return $"{GetType().Name}:> {_procName} {GetArgs()}";
        }

        protected virtual Process CreateProcess()
        {
            if (!File.Exists(ProgramPath))
                throw new FileNotFoundException("Program could not be found", ProgramPath);

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = ProgramPath,
                    Arguments = GetArgs(),

                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,

                },

                EnableRaisingEvents = true
            };
            proc.Exited += (ps, pe) =>
            {
                Log.Debug("{0} exit code: {1}", _procName, (ps as Process).ExitCode);
            };

            Log.Debug("cmd:> {0} {1}", _procName, proc.StartInfo.Arguments);

            return proc;
        }

        protected Task<int> RunProcAsync(Process proc, CancellationToken token = default)
        {
            var tcs = new TaskCompletionSource<int>();

            var pTask = proc.StartAsync(true, true, token);

            pTask.ContinueWith(t =>
            {
                ExitCode = t.Result.ExitCode;
                StdOutput = t.Result.StdOutput;
                StdError = t.Result.StdError;

                tcs.SetResult(ExitCode);
            });

            return tcs.Task;
        }

        protected abstract string GetArgs();

        // 0=Process name, 1=Exit Code, 2=Std Error, 3=Std Output
        protected const string REPORT_FMT = "{0} exited w/ code {1}\n\n" +
                                            "Std Error:\n{2}\n\n" +
                                            "Std Output:\n{3}\n";

        string _procName, _rfn, _progPath;
    }
}
