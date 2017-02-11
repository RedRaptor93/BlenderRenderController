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

        public static string fixEmpty(string conf)
        {
            if ((conf == null) || (conf == ""))
            { return "unset"; }
            else
            { return conf; }
        }

    }


}
 //if (
 //               ((set.ffmpeg_path != null) || (set.ffmpeg_path != "") || (set.ffmpeg_path != set.def_ffmpeg)) 
 //               ((set.blender_path != null) || (set.blender_path != "") || (set.blender_path != set.def_blender))
 //               )
 //           {
 //               // use user setting
 //               // ffmpeg
 //               return;