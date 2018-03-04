# Part of Blender Render Controller
# https://github.com/RedRaptor93/BlenderRenderController
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

active_scene_name = str(scene).partition('("')[-1].rpartition('")')[0]

res_p = scene.render.resolution_percentage
resolution = "{0} x {1}".format(math.floor(scene.render.resolution_x * res_p / 100), 
								math.floor(scene.render.resolution_y * res_p / 100))

fps = scene.render.fps / scene.render.fps_base

b_output = bpy.path.abspath(scene.render.filepath)
output_path = os.path.abspath(b_output)

print("Building data...")

blend_data = {
	'projectName': project_name,
	'sceneActive': active_scene_name,
	'start': scene.frame_start,
	'end': scene.frame_end,
	'fps': fps,
	'resolution': resolution,
	'outputPath': output_path,
	'fileFormat': scene.render.image_settings.file_format,
	'ffmpegFmt': scene.render.ffmpeg.format,
	'ffmpegCodec': scene.render.ffmpeg.codec,
	'ffmpegAudio': scene.render.ffmpeg.audio_codec
};

print(json.dumps(blend_data, indent=4, skipkeys=True))
