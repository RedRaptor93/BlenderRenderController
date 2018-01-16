<<<<<<< HEAD
# Blender Render Controller

## What is this?
Blender Render Controller is a tool to help speed up the render process in Blender's Video Sequence Editor(VSE).

VSE is pretty good for editing videos, it's precise and relatively easy to learn, making it a compelling choice next to other free video editing tools. There are some downsides too, main of which been that the renderer is SINGLE THREADED. Meaning that it won't take full advantage of all logical cores in your system, so rendering your finished project is SUPER SLOW compared to other video editors.

This tool offers a work-around until the Blender developers make a better renderer for VSE, 

This tool offers a work-around by calling multiple instances of `blender.exe`, each rendering a different segment of the project at the same time, making use of processing power that would otherwise go unused. After all parts are rendered, join them together and BAM, your video is ready much faster then previously possible.

## How much difference does it make?
Quite a lot! I did some testing shown below (Blender Render Controller shown in orange):

![Test3](https://app.box.com/representation/file_version_147671500287/image_2048/1.png?shared_name=u90snyjbzslz0zszwges1helzmyz6b8y)

![Test1](https://app.box.com/representation/file_version_147672318497/image_2048/1.png?shared_name=i1bwfn03tie6ieehwnz7mbp4lu700gzy)

PC used: i7 4790, 16GB DDR3 RAM @ 1600Mhz

Really shows the importance of those extra cores huh? Even if you don't use Blender VSE often, that’s a LOT of time saved. And the time added by joining the videos together is negligible (less then 1min).

## HOW TO USE

### Dependencies
- Blender, obviously
- FFmpeg, required for joining the parts together.

1. Save your .blend file with the settings you want (output path, resolution, etc)

	- Make sure the "output path" is an ABSOLUTE path and not relative, you can change the default kind of path in Blender's user settings, BRC WON'T work w/ relative paths
	
2. Open BlenderRenderController, browse for the desired blend file

	- Alternatively, you can specify a .blend file automatically in CMD: `> BlenderRenderController.exe “filepath to .blend`
	
3. Select the chunk of the segment you want to render and press "render segment" to render a single segment or select "render all" to render the project in segments.

	- The length of each segment is controlled by the difference between the "Start Frame" and "End frame" values, the default length when you open or re-read a file can be adjusted ("segment length" in Options => Settings)
	
4. When all the parts are done, click "Concatenate parts" to join all parts together

	- If you get a "Can't find working folder error", try "remove file from path" option below, "parts folder" must point to a FOLDER, not a FILE.
	
5. That's it!

## CHANGES

#### 21/02/17

- Updated README
- Program now stores settings
- Location of blender.exe and ffmpeg.exe can now be set manually

#### 03/02/17

- Upload to GitHub
- blend.json is deleted when program closes
- CMD arguments! Running the following in a CMD window will automatically set to specified file: 
```
BlenderRenderController.exe “filepath to .blend”
```
- added error message if blendFilePath does not exist

#### 02/02/17

- Added detailed error messages

- Render and join buttons are disabled if an error is detected

- Added icon

#### 29/01/17

- the program now reads project's info from blend file's ACTIVE SCENE, and shows the name and number of total scenes

- fixed some unhanded exceptions evolving output file paths

- different bars for blend output and ffmpeg join

  - External programs cannot easily change the output file name or location of the render, so there is now a separated bar showing the       output location where blender will render and a tooltip informing to change the location in Blender.
  
- [get_project_info.py] now saves the json file in the scripts folder, this is necessary to avoid exceptions if output path in blend       file is NOT valid.

- Added tooltips, a strip menu w/ some options, plus general adjustments to UI to accommodate new features

## TO DO

Note that I'm still a learner, I don't know how to do most of these things, so don't expect them anytime soon, of course if anyone is willing to help, I'm all ears.

- [x] Automatically calculate segment’s end frame

- Add an INI file to save settings
	- Project history
	- Set own default values for process count, frame step*
	- [x] ability to point to blender.exe and Ffmpeg.exe, eliminating the need to set PATH (?)

- Make a more precise timer

- [x] How to use section

- [x] *Change how segments are calculated, on top of a “start” and “end” frame, a frame step value – would control the segments length (end_frame = start_frame + frame_step)

- [x] find and delete json file on start up or closure

- Integrate w/ Blender via plugin

- [x] call Render Controller and pass project’s info automatically (command line args)

- Support for more file formats
=======
# Blender Render Controller 

## Download &#8615;
Latest release [HERE](https://github.com/jendabek/BlenderRenderController/releases/latest).

BRC is a tool to speed up the rendering process of videos in Blender's Video Sequence editor by rendering multiple diferent chunks of the video at the same time and joining them toguether after.

Check the wiki for more details about using and installing / building BRC.

Any questions, bug reports or suggestions? [Let us know](https://github.com/jendabek/BlenderRenderController/issues)!


### Known Issues

- *"Fatal python error"* when opening projects in Windows 7
	- Blender crashes when BRC requests project info.
	- Work-around: Launch _BlenderRenderController.exe_ from the command line, you can get a convenient _.bat_ file [here](https://github.com/jendabek/BlenderRenderController/blob/master/extras/utilities/runWin7.bat).

- Older versions of FFmpeg may fail to join chunks if AAC audio is used ([link](https://trac.ffmpeg.org/wiki/Encode/AAC#NativeFFmpegAACencoder))

- Ui may not update progress properly on Linux

## CREDITS

- Isti115
- meTwentyFive
- RedRaptor93
- jendabek

## Support the development &#9829;
<a href="https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=9SGQVK6TK2UJG&lc=US&item_name=Donation%20for%20Blender%20Render%20Controller&item_number=BRC&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted">
<img align="left" src="https://github.com/jendabek/BlenderRenderController/blob/master/extras/imgs/donate-github.png" width="110"/>
</a>
>>>>>>> 728f0236885b42c1260ec51d33dc2af27b515bef
