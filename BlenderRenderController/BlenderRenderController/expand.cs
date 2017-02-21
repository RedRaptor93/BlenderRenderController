using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlenderRenderController
{
    public static class ConfigBRC
    {
        /// <summary>
        /// Expands environment variables and, if unqualified, locates the exe in the working directory
        /// or the evironment's path.
        /// </summary>
        /// <param name="exe">The name of the executable file</param>
        /// <returns>The fully-qualified path to the file</returns>
        /// <exception cref="System.IO.FileNotFoundException">Raised when the exe was not found</exception>
        public static string FindExePath(string exe)
        //public static bool FindExePath(string exe)
        {
            exe = Environment.ExpandEnvironmentVariables(exe);
            if (!File.Exists(exe))
            {
                if (Path.GetDirectoryName(exe) == String.Empty)
                {
                    foreach (string test in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(';'))
                    {
                        string path = test.Trim();
                        if (!String.IsNullOrEmpty(path) && File.Exists(path = Path.Combine(path, exe)))
                            return Path.GetFullPath(path);
                            //return true;
                    }
                }
                throw new FileNotFoundException(new FileNotFoundException().Message, exe);
            }
            return Path.GetFullPath(exe);
            //return false;
        }

        /// <summary>
        /// Compare a setting and it's default, if setting is empty or equal to default, return true
        /// </summary>
        /// <param name="conf">User accessible setting</param>
        /// <param name="def">Default if conf is empty</param>
        /// <returns>True if conf is empty or equal def, else false</returns>
        public static bool isDef(string conf, string def)
        {
            if ((conf == null) || (conf == "") || (conf == def))
            { return true; }
            else
            { return false; }
        }

        /// <summary>
        /// Looks for blender.exe and ffmpeg.exe in PATH or in the user selected dir. If both fail, throw FileNotFoundException
        /// </summary>
        public static void EXECheck()
        {
            Properties.Settings set = Properties.Settings.Default;
            
            // FOR TESTING
            //set.blender_path = "C:\\Program Files\\Blender Foundation\\Blender\\blender.exe";
            //set.ffmpeg_path = "WRONG";

            // Fix null or empty sets
            var p1 = set.blender_path;
            var p2 = set.ffmpeg_path;

            if ((p1 == "PATH%") || (p1 == null) || (p1 == ""))
            {
                p1 = set.def_blender;
            }
            if ((p2 == "PATH%") || (p2 == null) || (p2 == ""))
            {
                p2 = set.def_ffmpeg;
            }

            string[] p10 = { p1, p2 };

            foreach (string item in p10)
            {
                if (!File.Exists(item))
                {
                    var test = item;
                    try
                    {
                        FindExePath(item);
                    }
                    catch (FileNotFoundException)
                    {
                        // Execs NOT in PATH, check if user defined is valid
                        // Exec INVALID
                        throw new FileNotFoundException("Could not find necessary programs", "blender and/or ffmpeg");
                        //errorMsgs(-25);
                    }
                }
            }
            // save any changes
            set.blender_path = p1;
            set.ffmpeg_path = p2;
        }

    }

}
