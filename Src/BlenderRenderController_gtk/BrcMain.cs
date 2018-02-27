using System;
using System.Diagnostics;
using System.IO;
using Gtk;

using UI = Gtk.Builder.ObjectAttribute;

namespace BlenderRenderController
{
    public class BrcMain : GtkWindow
    {
        #region Ui decl
        #pragma warning disable 169, 649
        [UI] Stack startStopStack;

        [UI] Button btnStartRender, btnStopRender;

        [UI] Adjustment 
            numChunkSizeAdjust,
            numEndFrameAdjust,
            numProcMaxAdjust;

        [UI] RecentManager recentBlendsManager;

        [UI] ProgressBar workProgress;

        [UI] Label 
            lblStatus,
            lblTimeElapsed,
            lblETR;

        [UI] Frame 
            frBlendFile,
            frRenderOptions, 
            frOutputFolder;

        [UI] MenuBar menuBar;

        [UI] Toolbar toolbar;

        #pragma warning restore 169, 649
        #endregion


        public BrcMain() : base("BrcGtk.glade", "brc_style.css", "BrcMain")
        {

        }


        // Events handlers
        private void On_OpenFile(object sender, EventArgs e)
        {
            
        }

        private void On_ReloadFile(object sender, EventArgs e)
        {

        }

        private void On_UnloadFile(object sender, EventArgs e)
        {
            
        }

        private void On_Preferences(object sender, EventArgs e)
        {
            
        }

        void On_miGithub_Clicked(object s, EventArgs e)
        {
            Process.Start("https://github.com/RedRaptor93/BlenderRenderController");
        }

        private void On_Quit(object s, EventArgs e)
        {
            //Close();
            Application.Quit();
        }

        void on_cbJoiningAction_changed(object s, EventArgs e)
        {

        }

        void on_cbRenderer_changed(object s, EventArgs e)
        {

        }

        void On_AutoStartStop_Toggled(object s, EventArgs e)
        {

        }

        void On_AutoChunkSize_Toggled(object s, EventArgs e)
        {

        }

        void On_numFrameRange_ChangingValue(object s, ChangeValueArgs e)
        {

        }

        void On_numFrameRange_ValueChanged(object s, EventArgs e)
        {

        }

        void StartRender_Clicked(object s, EventArgs e)
        {
            // start render...

            // startStopStack.SetVisibleChildFull("btnStopRender", StackTransitionType.None);
            startStopStack.VisibleChildName = "btnStopRender";
        }

        void StopRender_Clicked(object s, EventArgs e)
        {
            // stop / cancel render...

            // startStopStack.SetVisibleChildFull("btnStartRender", StackTransitionType.None);
            startStopStack.VisibleChildName = "btnStartRender";
        }

        void BtnChangeFolder_Clicked(object s, EventArgs e)
        {
            
        }

        void BtnOpenFolder_Clicked(object s, EventArgs e)
        {
            
        }

        void On_ShowAbout(object s, EventArgs e)
        {

        }
    }
}
