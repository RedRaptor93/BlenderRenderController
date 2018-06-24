// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using BRClib;
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
                var chunkSDir = subDirs.FirstOrDefault(di => di.Name == "chunks");
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
        
        public static MessageBoxButtons ToMsgBoxBtns(this VMDialogButtons dialogButtons)
        {
            MessageBoxButtons retval;
            switch (dialogButtons)
            {
                case VMDialogButtons.OK:
                    retval = MessageBoxButtons.OK;
                    break;
                case VMDialogButtons.OKCancel:
                    retval = MessageBoxButtons.OKCancel;
                    break;
                case VMDialogButtons.YesNo:
                    retval = MessageBoxButtons.YesNo;
                    break;
                case VMDialogButtons.RetryCancel:
                    retval = MessageBoxButtons.RetryCancel;
                    break;
                default:
                    goto case VMDialogButtons.OK;
            }
            return retval;
        }

        static VMDialogResult ToDR(this DialogResult result)
        {
            VMDialogResult retval;
            switch (result)
            {
                case DialogResult.OK:
                    retval = VMDialogResult.Ok;
                    break;
                case DialogResult.Cancel:
                    retval = VMDialogResult.Cancel;
                    break;
                case DialogResult.Yes:
                    retval = VMDialogResult.Yes;
                    break;
                case DialogResult.No:
                    retval = VMDialogResult.No;
                    break;
                case DialogResult.Retry:
                    retval = VMDialogResult.Retry;
                    break;
                default:
                    goto case DialogResult.OK;
            }
            return retval;
        }

        public static VMDialogResult ShowVMDialog(string title, string message, string details, VMDialogButtons buttons)
        {
            var btns = buttons.ToMsgBoxBtns();
            var icon = title.ToLower().StartsWith("w") ? MessageBoxIcon.Warning : MessageBoxIcon.Error;
            VMDialogResult retval;

            if (details != null)
            {
                var dlg = new Ui.DetailedMessageBox(message, title, details, btns, icon);
                retval = dlg.ShowDialog().ToDR();
            }
            else
            {
                retval = MessageBox.Show(message, title, btns, icon).ToDR();
            }

            return retval;
        }
    }

    //class Constants
    //{
    //    public const string ChunksSubfolder = "chunks";
    //    public const string APP_TITLE = "Blender Render Controller";
    //    public const string ChunksTxtFileName = "chunklist.txt";
    //}

}
