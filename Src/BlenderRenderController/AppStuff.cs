﻿// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence


using System;
using System.IO;

namespace BlenderRenderController
{
    class Constants
    {
        public const string PyGetInfo = "get_project_info.py";
        public const string PyMixdown = "mixdown_audio.py";
        public const string ChunksSubfolder = "chunks";
        public const string ScriptsSubfolder = "Scripts";
        public const string APP_TITLE = "Blender Render Controller";
        public const string ChunksTxtFileName = "chunklist.txt";
    }

    static class Dirs
    {
        static readonly string _appDataBase = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static readonly string AppData = Path.Combine(_appDataBase, "BlenderRenderController");
    }
    
}
