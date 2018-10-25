// Part of the Blender Render Controller project
// https://github.com/rehdi93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;


namespace BRClib.Extentions
{
    public static class Extentions
    {
        /// <summary>
        /// Gets the combined lenght of all <see cref="Chunk"/>s in a 
        /// collection
        /// </summary>
        /// <param name="chunks"></param>
        /// <returns></returns>
        public static int TotalLength(this IEnumerable<Chunk> chunks)
        {
            int len = 0;

            foreach (var chunk in chunks)
            {
                len += chunk.Length;
            }

            return len;
        }

        public static Chunk GetFullRange(this IEnumerable<Chunk> chunks)
            => new Chunk(chunks.First().Start, chunks.Last().End);


        /// <summary>
        /// Starts a process asynchronously and optionally reads its standard output and error streams
        /// </summary>
        /// <param name="token">Cancelation token, calls <see cref="Process.Kill()"/></param>
        /// <param name="getStdOut">If set to true, this method will read the Std Output and
        /// save its contents in <see cref="ProcessResult.StdOutput"/>, otherwise it will be null</param>
        /// <param name="getStdErr">If set to true, this method will read the Std Error and
        /// save its contents in <see cref="ProcessResult.StdError"/>, otherwise it will be null</param>
        /// <returns>A <see cref="ProcessResult"/> object with the exit code and, optionally, its 
        /// standard output and standard error contents as strings</returns>
        public async static Task<ProcessResult> StartAsync(this Process proc,
                                                           bool getStdOut,
                                                           bool getStdErr,
                                                           CancellationToken token = default)
        {
            proc.StartInfo.RedirectStandardOutput = getStdOut;
            proc.StartInfo.RedirectStandardError = getStdErr;

            Task<int> eCodeTask;
            Task<string> soTask, seTask;

            using (token.Register(ProcCancelCallback, proc))
            {
                eCodeTask = RunProcessAsync(proc);
                soTask = getStdOut ? proc.StandardOutput.ReadToEndAsync() : Task.FromResult<string>(null);
                seTask = getStdErr ? proc.StandardError.ReadToEndAsync() : Task.FromResult<string>(null);

                await Task.WhenAll(eCodeTask, soTask, seTask).ConfigureAwait(false);
            }

            return new ProcessResult(eCodeTask.Result, soTask.Result, seTask.Result);
        }

        /// <summary>
        /// Starts a process asynchronously
        /// </summary>
        /// <param name="token">Cancelation token, calls <see cref="Process.Kill()"/></param>
        /// <returns>A <see cref="ProcessResult"/> object with the exit code and, optionally, its 
        /// standard output and standard error contents as strings</returns>
        public async static Task<ProcessResult> StartAsync(this Process proc, CancellationToken token = default)
        {
            bool getStdOut = proc.StartInfo.RedirectStandardOutput;
            bool getStdError = proc.StartInfo.RedirectStandardError;

            Task<int> eCodeTask;
            Task<string> soTask, seTask;

            using (token.Register(ProcCancelCallback, proc))
            {
                eCodeTask = RunProcessAsync(proc);
                soTask = getStdOut ? proc.StandardOutput.ReadToEndAsync() : Task.FromResult<string>(null);
                seTask = getStdError ? proc.StandardError.ReadToEndAsync() : Task.FromResult<string>(null);

                await Task.WhenAll(eCodeTask, soTask, seTask).ConfigureAwait(false);
            }

            return new ProcessResult(eCodeTask.Result, soTask.Result, seTask.Result);
        }

        private static Task<int> RunProcessAsync(Process proc)
        {
            var tcs = new TaskCompletionSource<int>();

            if (!proc.EnableRaisingEvents)
            {
                throw new InvalidOperationException("Cannot run process asynchronously, " +
                    "'EnableRaisingEvents' must be set to true");
            }

            proc.Exited += (s, e) => tcs.SetResult((s as Process).ExitCode);

            // Not sure about the guarantees of the Exited event in case of
            // process reuse
            bool started = proc.Start();
            if (!started)
            {
                throw new InvalidOperationException($"Could not start process: {proc}.\n" +
                    "Obs: Reusing existing processes is not supported.");
            }

            return tcs.Task;
        }

        private static void ProcCancelCallback(object obj)
        {
            if (obj is Process proc)
            {
                try
                {
                    proc.Kill();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, category: "Proc Dispose Error");
                }
                finally
                {
                    proc.Dispose();
                }
            }
        }
    }

    /// <summary>
    /// The result of a process that ran asynchronously
    /// </summary>
    public class ProcessResult
    {
        public ProcessResult() { }

        public ProcessResult(int exitCode) : this(exitCode, null, null) { }

        public ProcessResult(int exitCode, string stdOutput, string stdError)
        {
            ExitCode = exitCode;
            StdOutput = stdOutput;
            StdError = stdError;
        }

        public int ExitCode { get; }
        public string StdOutput { get; }
        public string StdError { get; }
    }

}
