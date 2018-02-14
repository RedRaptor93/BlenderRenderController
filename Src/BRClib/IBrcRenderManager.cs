// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BRClib
{
    public interface IBrcRenderManager
    {
        bool InProgress { get; }
        bool WasAborted { get; }
        AfterRenderAction Action { get; }
        Renderer Renderer { get; }

        void Setup(Project project, AfterRenderAction action, Renderer renderer);
        void Setup(Project project);
        void StartAsync();
        void Abort();

        event EventHandler<RenderProgressInfo> ProgressChanged;
        event EventHandler<AfterRenderAction> AfterRenderStarted;
        event EventHandler<BrcRenderResult> Finished;
    }
}
