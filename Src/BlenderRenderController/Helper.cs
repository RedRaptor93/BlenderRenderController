// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BlenderRenderController
{
    static class Helper
    {
        private static Logger logger = LogManager.GetLogger("Helper");

        static public bool ClearOutputFolder(string path)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                DirectoryInfo[] subDirs = dir.GetDirectories();

                // clear files in the output
                foreach (FileInfo fi in dir.GetFiles())
                {
                    fi.Delete();
                }

                // clear files in the 'chunks' subdir
                var chunkSDir = subDirs.FirstOrDefault(di => di.Name == Constants.ChunksSubfolder);
                if (chunkSDir != null)
                {
                    Directory.Delete(chunkSDir.FullName, true);
                }

                return true;
            }
            catch (IOException)
            {
                string msg = "Can't clear output folder, files are in use";
                logger.Error(msg);
                MessageBox.Show(msg);
                return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Trace(ex.StackTrace);
                MessageBox.Show("An unexpected error ocurred, sorry.\n\n" + ex.Message);
                return false;
            }
        }
        
        public static void SetToolTip(this ToolTip toolTip, Control control, string caption, bool recursive)
        {
            if (string.IsNullOrEmpty(caption))
                return;

            toolTip.SetToolTip(control, caption);

            if (recursive)
            {
                foreach (Control subItem in control.Controls)
                {
                    toolTip.SetToolTip(subItem, caption);
                }
            }
        }
        
    }
}
