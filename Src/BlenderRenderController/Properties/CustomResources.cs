// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;
using System.Collections.Generic;
using BRClib;

namespace BlenderRenderController.Properties
{
    internal class CustomRes
    {
        public static readonly Dictionary<AfterRenderAction, string> AfterRenderResources =
            new Dictionary<AfterRenderAction, string>
            {
                [AfterRenderAction.MIX_JOIN] = Resources.AR_JoinMixdown,
                [AfterRenderAction.JOIN] = Resources.AR_JoinOnly,
                [AfterRenderAction.NOTHING] = Resources.AR_NoAction
            };


        public static readonly Dictionary<Renderer, string> RendererResources =
            new Dictionary<Renderer, string>
            {
                [Renderer.BLENDER_RENDER] = Resources.Renderer_Blender,
                [Renderer.CYCLES] = Resources.Renderer_Cycles
            };
    }
}