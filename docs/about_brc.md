## What is this?
<img align="right" src="https://github.com/rehdi93/BlenderRenderController/blob/master/extras/imgs/blender-render-controller.png" width="480"/>
Blender Render Controller is a tool to help speed up the rendering of Blender's Video Sequence Editor (VSE) projects.

VSE is pretty good for editing videos, it's precise and relatively easy to learn, making it a compelling choice next to other free video editing tools. There are some downsides too, main of which been that the renderer is **single threaded**. Meaning that it won't take full advantage of all logical cores in your system, so rendering your finished project is **super slow** compared to other video editors.

This tool offers a work-around for this limitation until the Blender developers make a better renderer for VSE. 

It renders different segments (chunks) of the project at the same time by calling multiple blender.exe instances, **making use of full processing power of your PC**.
After all parts are rendered, they're joined together in FFmpeg, and your **video is ready much faster** then previously possible.

### Video demonstration
[<img src="https://github.com/rehdi93/BlenderRenderController/blob/master/extras/imgs/intro-720.png" width="480"/>](https://www.youtube.com/watch?v=Kdvq1CzOPfM)

## How much difference does it make?
**Quite a lot!** I did some testing shown below (Blender Render Controller shown in orange):

![Test3](https://github.com/rehdi93/BlenderRenderController/blob/master/extras/imgs/brc%20graph%201080p.png)

![Test1](https://github.com/rehdi93/BlenderRenderController/blob/master/extras/imgs/brc%20graph%20720p.png)

PC used: i7 4790, 16GB DDR3 RAM @ 1600Mhz

Really shows the importance of those extra cores huh? And of course, more processor cores = bigger difference.
Even if you don't use Blender VSE often, that’s a LOT of time saved. And the time added by joining the videos together is negligible (less then 1min).

Make sure to check the rest of the wiki for tips on how to get the most of BRC
