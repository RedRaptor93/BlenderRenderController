# Part of Blender Render Controller
# https://github.com/rehdi93/BlenderRenderController
# Copyright 2017-present Pedro Oliva Rodrigues
# This code is released under the MIT licence

import os
import json
import bpy
import math

print('Requesting infos...')

blend_path = bpy.context.blend_data.filepath
project_name = bpy.path.display_name_from_filepath( blend_path )

scene = bpy.context.scene

res_p = scene.render.resolution_percentage
resolution = "{0} x {1}".format(math.floor(scene.render.resolution_x * res_p / 100), 
								math.floor(scene.render.resolution_y * res_p / 100))

fps = scene.render.fps / scene.render.fps_base

output = os.path.abspath(bpy.path.abspath(scene.render.filepath))
output_path, file = os.path.split(output)

print("Building data...")

blend_data = {
	'projectName': project_name,
	'sceneActive': scene.name,
	'start': scene.frame_start,
	'end': scene.frame_end,
	'fps': fps,
	'resolution': resolution,
	'outputPath': output_path,
    'outputFile': file,
	'fileFormat': scene.render.image_settings.file_format,
	'ffmpegFmt': scene.render.ffmpeg.format,
	'ffmpegCodec': scene.render.ffmpeg.codec,
	'ffmpegAudio': scene.render.ffmpeg.audio_codec
};

print(json.dumps(blend_data, indent=2, skipkeys=True))
