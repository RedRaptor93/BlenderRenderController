﻿using BRClib;
using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace BlenderRenderController
{
    partial class BrcMain
    {
        #region Ui declarations
        #pragma warning disable 169, 649

        // main frames
        [UI] Frame frBlendFile, frRenderOptions, frOutputFolder;
        [UI] Box frameRangeBox, chunkDivBox;

        [UI] Stack startStopStack;
        [UI] Button btnStartRender, btnStopRender;

        [UI] Adjustment numStartFrameAdjust, numEndFrameAdjust, 
            numChunkSizeAdjust,  numProcMaxAdjust;

        // status bar
        [UI] ProgressBar workProgress;
        [UI] Spinner workSpinner;
        [UI] Label lblStatus, lblTimeElapsed, lblETR;

        // info box values
        [UI] Label activeSceneInfoValue, durationInfoValue, fpsInfoValue, resolutionInfoValue;

        // menubar
        [UI] MenuBar menuBar;

        // -File
        [UI] ImageMenuItem miOpenFile, miReloadFile, miPref;
        [UI] MenuItem miUnload, miOpenRecent;

        // -Tools
        [UI] ImageMenuItem miRenderMixdown, miJoinChunks;

        // toolbar
        [UI] Toolbar toolbar;
        [UI] ToolButton tsOpenFile, tsReloadFile, tsAbout;
        [UI] MenuToolButton tsOpenRecent;

        [UI] ComboBox cbJoiningAction, cbRenderer;

        [UI] Entry entryOutputPath;

        #pragma warning restore 169, 649
        #endregion

        FileChooserDialog openBlendDialog, chooseOutputFolderDialog;
        RecentChooserMenu recentBlends;
        RecentChooserWidget chooserWidget;
        MenuItem recentBlendsMenu;

        void Initialize()
        {
            // setup filters
            string blend = "*.blend";

            var blendFilter = new FileFilter();
            blendFilter.Name = ".blend files";
            blendFilter.AddPattern(blend);

            var recentBlendsFilter = new RecentFilter();
            recentBlendsFilter.Name = blendFilter.Name;
            recentBlendsFilter.AddPattern(blend);


            openBlendDialog = new FileChooserDialog("Open blend file", this, FileChooserAction.Open,
                "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);

            openBlendDialog.Filter = blendFilter;

            chooseOutputFolderDialog = new FileChooserDialog("Choose output folder", this, FileChooserAction.SelectFolder,
                "Cancel", ResponseType.Cancel, "Select", ResponseType.Accept);

            //chooserWidget = new RecentChooserWidget();
            //chooserWidget.Filter = recentBlendsFilter;

            recentBlends = new RecentChooserMenu();
            recentBlends.Filter = recentBlendsFilter;
            //recentBlends.ActivateCurrent += On_OpenRecent;
            recentBlends.AddSignalHandler("item-activated", new EventHandler(On_OpenRecent));

            miOpenRecent.Submenu = recentBlends;
            tsOpenRecent.Menu = recentBlends;

            numProcMaxAdjust.Value =
            numProcMaxAdjust.Upper = Environment.ProcessorCount;

            frameRangeBox.Sensitive = chunkDivBox.Sensitive = false;
            AutoFrameRange = AutoChunkDiv = true;

            SetupHandlers();
        }

        private void SetupHandlers()
        {
            RecentManager.Default.Changed += RecentMngr_Changed;
            //miRecentList.AddSignalHandler("item-activated", new EventHandler(On_RecentItem_Activated));
        }



        // private properties
        bool AutoFrameRange { get; set; }

        bool AutoChunkDiv { get; set; }
    }
}
