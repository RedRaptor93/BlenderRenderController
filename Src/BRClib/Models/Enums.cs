// Part of the Blender Render Controller project
// https://github.com/rehdi93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;
using System.Collections.ObjectModel;
using System.IO;

namespace BRClib
{
    public enum Renderer
    {
        BLENDER_RENDER, CYCLES
    }

    [Flags]
    public enum AfterRenderAction
    {
        NOTHING = 0,
        MIXDOWN = 1,
        JOIN = 2,
        MIX_JOIN = MIXDOWN | JOIN
    }

    public enum BrcRenderResult
    {
        AllOk,
        Aborted,
        AfterRenderFailed,
        ChunkRenderFailed
    }
}
