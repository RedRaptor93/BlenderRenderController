// Part of the Blender Render Controller project
// https://github.com/rehdi93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;
using System.Collections.Generic;
using BRClib;

namespace BlenderRenderController.Properties
{
    using BrcRes = BRClib.Properties.Resources;

    internal class CustomRes
    {
        public static readonly Dictionary<AfterRenderAction, string> AfterRenderResources =
            new Dictionary<AfterRenderAction, string>
            {
                [AfterRenderAction.MIX_JOIN] = BrcRes.AR_JoinMixdown,
                [AfterRenderAction.JOIN] = BrcRes.AR_JoinOnly,
                [AfterRenderAction.NOTHING] = BrcRes.AR_NoAction
            };


        public static readonly Dictionary<Renderer, string> RendererResources =
            new Dictionary<Renderer, string>
            {
                [Renderer.BLENDER_RENDER] = BrcRes.Renderer_Blender,
                [Renderer.CYCLES] = BrcRes.Renderer_Cycles
            };
    }
}