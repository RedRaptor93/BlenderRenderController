### Optimizing Chunk division:
If the project you're working on is just a few videos spliced together, there isn't any mystery, just set it to auto (even number of frames per chunk) and you're good to go. Because the VIDEO rendering of Blender is single threaded.

If some effects are in play, like "fade in"s and "fade out"s, overlays, changing colors, etc. These will use extra CPU power, sometimes on _multiple threads_.

You can compensate for this by setting a smaller chunk size, so as some processes are busy with those harder parts, more simple parts are in the queue and getting done.