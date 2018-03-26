// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Reflection;

namespace BRClib
{
    public class Project : BlendData
    {
        string _blendPath, _chunksDir;
        int _maxC, _chunkLen = 50;
        private ObservableCollection<Chunk> _chunkList;

        public Project()
        {
            _chunkList = new ObservableCollection<Chunk>();
        }

        public Project(BlendData blend)
        {
            Start = blend.Start;
            End = blend.End;
            Fps = blend.Fps;
            Resolution = blend.Resolution;
            OutputPath = blend.OutputPath;
            ProjectName = blend.ProjectName;
            ActiveScene = blend.ActiveScene;
            FileFormat = blend.FileFormat;
            FFmpegVideoFormat = blend.FFmpegVideoFormat;
            FFmpegAudioCodec = blend.FFmpegAudioCodec;

            MaxConcurrency = Environment.ProcessorCount;

            _chunkList = new ObservableCollection<Chunk>(Chunk.CalcChunks(Start, End, MaxConcurrency));

            ChunkLenght = _chunkList[0].Length;

            ChunksDirPath = DefaultChunksDirPath;
        }

        public string BlendFilePath
        {
            get => _blendPath;
            set => SetProperty(ref _blendPath, value);
        }

        public ObservableCollection<Chunk> ChunkList => _chunkList;

        public int ChunkLenght
        {
            get => _chunkLen;
            set => SetProperty(ref _chunkLen, value);
        }

        public int MaxConcurrency
        {
            get => _maxC;
            set => SetProperty(ref _maxC, value);
        }

        public string ChunksDirPath
        {
            get => _chunksDir;
            set => _chunksDir = value;
        }

        public string DefaultChunksDirPath
        {
            get
            {
                if (OutputPath == null)
                    return null;

                return Path.Combine(OutputPath, "chunks");
            }
        }

        public TimeSpan? Duration
        {
            get
            {
                var duration = (End - Start + 1) / Fps;
                if (!double.IsNaN(duration) && !double.IsInfinity(duration))
                    return TimeSpan.FromSeconds(duration);
                else
                    return null;
            }
        }

        public int? TotalFrames
        {
            get
            {
                if (End <= Start)
                {
                    return null;
                }
                return End - Start + 1;
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string pName = null)
        {
            base.OnPropertyChanged(pName);

            if (pName == nameof(Start) || pName == nameof(End))
            {
                OnPropertyChanged(nameof(Duration));
                OnPropertyChanged(nameof(TotalFrames));
            }

        }

    }
}
