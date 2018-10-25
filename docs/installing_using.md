## Installing

Before starting, you should have the following dependencies installed:

- Blender, obviously.
- [FFmpeg](https://ffmpeg.org/download.html), required for joining the parts together. 

### Windows
- .NET framework 4.6.1 or higher

There are 2 [installers](https://github.com/rehdi93/BlenderRenderController/releases/latest) available, the main difference being the 'portable' won't create shortcuts and has the portable mode enabled by default. **Portable mode** meaning BRC won't create files outside of its executable directory (except when rendering). Can safely be moved around.

You don't need to use the portable setup to get portable mode, you can edit the `BlenderRenderController.exe.config` and under 'appsettings', set "portable" to "true".

### Linux (beta)
- .NET core runtime 2.0 or higher
- GTK3 3.20 or higher
   
## Using
1. Create your Blender VSE project normally within Blender.
 
2. Open Blender Render Controller, browse for the .blend file.
 
3. BRC will automatically calculate the *Start Frame*, *End Frame* and *Chunk Size* according to the length of the project and number of logical cores in your CPU respectively, you can change these values manually if you want.

   - Normally, the N# of processes should match the N# of logical cores in you system for a optimum render performance.
 
4. Choose a joining method:

   - *Join chunks w/ mixdown audio* - renders chunks, makes a separated audio file and joins it all together, recommended if you have audio tracks in your project.
   - *Join chunks* - same as above, minus audio mixdown.
   - *No extra action* - just renders the Chunks.
 
5. Click *Start Render* and wait for the render to be done.
