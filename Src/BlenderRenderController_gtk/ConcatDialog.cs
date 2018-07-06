using System;
using System.Collections.Generic;
using Gtk;
using BRClib.Commands;
using UI = Gtk.Builder.ObjectAttribute;
using System.Linq;
using static BRClib.RenderFormats;
using System.IO;

namespace BlenderRenderController
{
    class ConcatDialog : Dialog
    {
        public ConcatDialog() : this(Glade.LoadUI("Dialogs.glade", "brc_style.css"), "ConcatDialog")
        {
            Initialize();
        }

        public ConcatCmd Concat { get; private set; }

        #region Ui declarations
        #pragma warning disable 169, 649, IDE0044

        [UI] FileChooserButton concatFileChooser, mixdownChooser;
        [UI] Button joinBtn, cancelBtn, setOutputBtn;
        [UI] Entry outputEntry;

        #pragma warning restore 169, 649, IDE0044
        #endregion

        FileChooserDialog outputChooserDialog;
        string _concatFile, _outputFile, _mixdownFile;

        private ConcatDialog(Tuple<Builder, CssProvider> elements, string root)
            : base(elements.Item1.GetObject(root).Handle)
        {
            elements.Item1.Autoconnect(this);
            StyleContext.AddProviderForScreen(Screen, elements.Item2, 800);
        }

        void Initialize()
        {
            outputChooserDialog = new FileChooserDialog("Select name and location of final output", this, FileChooserAction.Save);
            outputChooserDialog.AddButton(Stock.Lookup(Stock.SaveAs).Label, ResponseType.Accept);
            outputChooserDialog.AddButton(Stock.Lookup(Stock.Cancel).Label, ResponseType.Cancel);

            joinBtn.Sensitive = false;

            joinBtn.Clicked += JoinBtn_Clicked;
            cancelBtn.Clicked += CancelBtn_Clicked;
            setOutputBtn.Clicked += OutputBtn_Clicked;

            mixdownChooser.FileSet += MixdownChooser_FileSet;
            concatFileChooser.FileSet += ConcatFileChooser_FileSet;

            // setup filters
            foreach (var f in MakeChooserFilters(AudioFileExts))
                mixdownChooser.AddFilter(f);

            foreach (var f in MakeChooserFilters(VideoFileExts))
                outputChooserDialog.AddFilter(f);

            concatFileChooser.Filter = new FileFilter { Name = "FFmpeg's concat file" };
            concatFileChooser.Filter.AddMimeType("text/plain");
        }


        private void OutputBtn_Clicked(object sender, EventArgs e)
        {
            var result = (ResponseType)outputChooserDialog.Run();

            if (result == ResponseType.Accept)
            {
                _outputFile = outputEntry.Text = outputChooserDialog.Filename;
                joinBtn.Sensitive = ValidateFiles();
            }

            outputChooserDialog.Hide();
        }

        private void ConcatFileChooser_FileSet(object sender, EventArgs e)
        {
            _concatFile = concatFileChooser.Filename;
            joinBtn.Sensitive = ValidateFiles();
        }

        private void MixdownChooser_FileSet(object sender, EventArgs e)
        {
            _mixdownFile = mixdownChooser.Filename;
            joinBtn.Sensitive = ValidateFiles();
        }

        private void CancelBtn_Clicked(object sender, EventArgs e)
        {
            Respond(ResponseType.Cancel);
        }

        private void JoinBtn_Clicked(object sender, EventArgs e)
        {
            Concat = new ConcatCmd
            {
                ConcatTextFile = _concatFile,
                OutputFile = _outputFile,
                MixdownFile = _mixdownFile
            };

            Respond(ResponseType.Accept);
        }

        List<FileFilter> MakeChooserFilters(string[] extentions)
        {
            int entryCount = extentions.Length + 1;
            var filters = new List<FileFilter>(entryCount);

            // all files
            filters.Add(new FileFilter() { Name = "All files" });
            filters[0].AddPattern("*");

            for (int i = 0; i < entryCount - 1; i++)
            {
                var ext = extentions[i];
                var ff = new FileFilter { Name = ext.Substring(1).ToUpper() + " files" };
                ff.AddPattern("*" + ext);

                filters.Add(ff);
            }

            return filters;
        }

        bool ValidateFiles()
        {
            bool concatValid = concatFileChooser.File != null && concatFileChooser.File.Exists;
            bool outputValid = outputChooserDialog.File != null;
            // optional
            bool mixdownValid = mixdownChooser.File == null || mixdownChooser.File.Exists;

            return concatValid && outputValid && mixdownValid;
        }
    }
}
