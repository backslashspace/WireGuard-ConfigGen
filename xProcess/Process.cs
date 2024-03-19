using System.Diagnostics;
using System;

namespace BSS.Launcher
{
    internal static class xProcess
    {
        internal struct Result
        {
            public Result(Int32 exitCode, String stdout, String stderr)
            {
                ExitCode = exitCode;
                this.stdout = stdout;
                this.stderr = stderr;
            }

            internal Int32 ExitCode;
            internal String stdout;
            internal String stderr;
        }

        ///<summary>Launches executables.</summary>
        ///<exception cref="FileLoadException"></exception>
        internal static Result? Run(String path, String args = null, Boolean RunAs = false, Boolean redirectOutput = false, Boolean hiddenExecute = false, Boolean waitForExit = false, String workingDirectory = null)
        {
            using Process process = new();

            process.StartInfo.FileName = path;

            if (RunAs)
            {
                process.StartInfo.Verb = "runas";
            }

            if (hiddenExecute)
            {
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;
            }

            if (redirectOutput)
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
            }
            else
            {
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.RedirectStandardError = false;
            }

            if (workingDirectory != null)
            {
                process.StartInfo.WorkingDirectory = workingDirectory;
            }

            if (args != null)
            {
                process.StartInfo.Arguments = args;
            }

            process.Start();

            if (redirectOutput)
            {
                process.WaitForExit();

                return new(process.ExitCode, process.StandardOutput.ReadToEnd(), process.StandardError.ReadToEnd());
            }
            else if (waitForExit)
            {
                process.WaitForExit();

                return new Result(process.ExitCode, null, null);
            }

            return null;
        }
    }
}