## Installing

### Dependencies
- Blender, obviously.
- [FFmpeg](https://ffmpeg.org/download.html), required for joining the parts together. You don't need to worry about it if you download the Full version which has FFmpeg already included.
- On Windows:
   * .NET framework 4.5.2 or higher
- <s>On Linux:
  * Mono 4.8 or higher
  * gnome-themes-standard</s> **!read below!**


#### Obs: Linux and Mac:
<s> If your compiling from source, define *UNIX* as a compilation symbol so the it'll work w/ Mono </s>

Starting w/ *v1.1.0.0* Mono support is deprecated, releases will no longer have a "Unix" version and the `UNIX` conditional code will be removed in the future.

## Using
1. Create your Blender VSE project normally within Blender.
 
2. Open Blender Render Controller, browse for the .blend file.
 
3. BRC will automatically calculate the *Start Frame*, *End Frame* and *Chunk Size* according to the length of the project and number of logical cores in your CPU respectively, you can change these values manually if you want.

   - Normally, the N# of processes should match the N# of logical cores in you system for a optimum render performance.
 
4. Choose the render method:

   - *Join chunks w/ mixdown audio* - renders chunks, makes a separated audio file and joins it all together, recommended if you have audio tracks in your project.
   - *Join chunks* - same as above, minus audio mixdown.
   - *No extra action* - just renders the Chunks.
 
5. Click *Start Render* and wait for the render to be done.