﻿// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
        // 0=Process name, 1=Exit Code, 2=Std Error, 3=Std Output
        protected const string REPORT_FMT = "{0} exited w/ code {1}\n\n" +
                                            "Std Error:\n{2}\n\n" +
                                            "Std Output:\n{3}";

        string _stdErr, _stdOut, _procName, _rfn, _progPath;
        int _eCode;


        public string ProgramPath
        {
            get => _progPath;
            set
            {
                // account for relative paths
                var fullPath = Path.GetFullPath(value);

                if (!File.Exists(fullPath))
                    throw new FileNotFoundException();

                _progPath = fullPath;
            }
        }

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

        public string StdOutput
        {
            get => _stdOut;
            protected set => _stdOut = value;
        }

        public string StdError
        {
            get => _stdErr;
            protected set => _stdErr = value;
        }

        public int ExitCode
        {
            get => _eCode;
            protected set => _eCode = value;
        }

        public ExternalCommand(string programPath)
        {
            ProgramPath = programPath;
            Log = LogManager.GetLogger(GetType().FullName);
            _procName = Path.GetFileNameWithoutExtension(ProgramPath);
        }

        public Process GetProcess()
        {
            return CreateProcess(ProgramPath, GetArgs());
        }

        protected virtual Process CreateProcess(string program, string args)
        {

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = program,
                    Arguments = args,

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

        protected abstract string GetArgs();

        public virtual Task<int> RunAsync(CancellationToken token = default)
        {
            return RunProcAsync(GetProcess(), token);
        }

        protected Task<int> RunProcAsync(Process proc, CancellationToken token = default)
        {
            var tcs = new TaskCompletionSource<int>();

            var pTask = proc.StartAsync(true, true, token);

            pTask.ContinueWith(t =>
            {
                _eCode = t.Result.ExitCode;
                _stdOut = t.Result.StdOutput;
                _stdErr = t.Result.StdError;

                tcs.SetResult(ExitCode);
            },
            TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        public virtual void SaveReport(string folderPath)
        {
            var filePath = Path.Combine(folderPath, ReportFileName);

            string fmt = REPORT_FMT;
            using (var sw = File.AppendText(filePath))
            {
                sw.WriteLine(string.Format(fmt, _procName, ExitCode, 
                                            StdError, StdOutput));
                sw.Write("\n\n");
            }
        }

        public override string ToString()
        {
            return $"{GetType().Name}:> {_procName} {GetArgs()}";
        }
    }
}
