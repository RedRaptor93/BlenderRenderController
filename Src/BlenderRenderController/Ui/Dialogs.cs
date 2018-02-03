// For Mono compatible Unix builds compile with /d:UNIX

using System;
using System.Text;
using System.Windows.Forms;

using Microsoft.WindowsAPICodePack.Dialogs;


namespace BlenderRenderController.Ui
{
    static class Dialogs
    {
        public static DialogResult ShowErrorBox(string textBody, string mainText, string caption, string details)
        {
            var td = new TaskDialog();
            td.Text = textBody;
            td.InstructionText = mainText;
            td.Caption = caption;

            td.DetailsExpanded = false;
            td.DetailsExpandedText = details;
            td.ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter;

            td.Icon = TaskDialogStandardIcon.Error;
            td.FooterIcon = TaskDialogStandardIcon.Information;
            td.StandardButtons = TaskDialogStandardButtons.Ok;

            return td.Show().ToDR();
        }

        public static DialogResult ShowErrorBox(string textBody, string mainText, string details)
        {
            //return ShowErrorBox(textBody, mainText, null, details);
            var eb = new DetailedMessageBox(textBody, mainText, details);
            return eb.ShowDialog();
        }


        public static string OutputFolderSelection(string title, string initialDir)
        {
            var dialog = new CommonOpenFileDialog
            {
                InitialDirectory = initialDir,
                IsFolderPicker = true,
                Title = title,
            };

            var res = dialog.ShowDialog();
            string path = null;

            if (res == CommonFileDialogResult.Ok)
            {
                path = dialog.FileName;
            }

            return path;
        }

        static DialogResult ToDR(this TaskDialogResult tdr)
        {
            switch (tdr)
            {
                case TaskDialogResult.None:
                    return DialogResult.None;
                case TaskDialogResult.Ok:
                    return DialogResult.OK;
                case TaskDialogResult.Yes:
                    return DialogResult.Yes;
                case TaskDialogResult.No:
                    return DialogResult.No;
                case TaskDialogResult.Cancel:
                    return DialogResult.Cancel;
                case TaskDialogResult.Retry:
                    return DialogResult.Retry;
                case TaskDialogResult.Close:
                    return DialogResult.Abort;
                case TaskDialogResult.CustomButtonClicked:
                default:
                    throw new NotSupportedException("Task result not supported");
            }
        } 

    }
}
