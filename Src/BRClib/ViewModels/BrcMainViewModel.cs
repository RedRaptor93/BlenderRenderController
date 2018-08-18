using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BRClib.Render;
using BRClib.Extentions;
using MvvmHelpers;
using static BRClib.Global;

namespace BRClib.ViewModels
{
    public class BrcMainViewModel : BaseViewModel
    {
        public BrcMainViewModel()
        {
            Title = "Blender Render Controller";
            Header = "blend name"; // project name
            Footer = "Ready"; // Status
            CanLoadMore = false; // Can load blend

            Chunks = new List<Chunk>();
            _bkpRange = new Chunk(1, 50);
            _data = new BlendData();

            _startFrame = _bkpRange.Start;
            _endFrame = _bkpRange.End;
            _maxProcs = Environment.ProcessorCount;

            _autoChunkSize =
            _autoFrameRange =
            _autoMaxProcs = true;

            _renderMngr = new RenderManager();
            _renderMngr.Finished += RenderManager_Finished;
            _renderMngr.AfterRenderStarted += RenderManager_AfterRenderStarted;
            _renderMngr.ProgressChanged += RenderManager_ProgressChanged;

            _etaCalc = new ETACalculator(10, 5);
        }



        public bool ConfigOk
        {
            get => _configOK;
            set
            {
                if (SetProperty(ref _configOK, value))
                {
                    CanLoadMore = _configOK;
                }
            }
        }

        public BlendData Data
        {
            get => _data;
            private set
            {
                SetProperty(ref _data, value, nameof(Data), OnDataUpdated);
            }
        }

        public string BlendFile { get; private set; }

        public List<Chunk> Chunks { get; set; }

        public string ActiveScene { get; set; }

        public TimeSpan Duration
        {
            get
            {
                double d = (StartFrame - EndFrame + 1) / FPS;
                if (!double.IsNaN(d) && !double.IsInfinity(d))
                    return TimeSpan.FromSeconds(d);

                return TimeSpan.Zero;
            }
        }


        public double FPS { get; set; }
        public string Resolution { get; set; }

        public int StartFrame
        {
            get => _startFrame;
            set => SetProperty(ref _startFrame, value);
        }

        public int EndFrame
        {
            get => _endFrame;
            set => SetProperty(ref _endFrame, value);
        }

        public int ChunkSize
        {
            get => _chunkSize;
            set
            {
                if (AutoChunkSize) return;

                SetProperty(ref _chunkSize, value);
            }
        }

        public int MaxProcessors
        {
            get => _maxProcs;
            set
            {
                if (AutoMaxProcessors) return;

                SetProperty(ref _maxProcs, value);
            }
        }

        public bool AutoChunkSize
        {
            get => _autoChunkSize;
            set
            {
                if (SetProperty(ref _autoChunkSize, value))
                {
                    if (_autoChunkSize)
                    {
                        if (Chunks.Count > 0)
                        {
                            ChunkSize = Chunks[0].Length;
                        }
                    }
                }
            }
        }

        public bool AutoMaxProcessors
        {
            get => _autoMaxProcs;
            set
            {
                if (SetProperty(ref _autoMaxProcs, value))
                {
                    if (_autoMaxProcs)
                    {
                        MaxProcessors = Environment.ProcessorCount;
                    }
                }
            }
        }

        public bool AutoFrameRange
        {
            get => _autoFrameRange;
            set
            {
                if (SetProperty(ref _autoFrameRange, value))
                {
                    if (_autoFrameRange)
                    {
                        StartFrame = _bkpRange.Start;
                        EndFrame = _bkpRange.End;
                    }
                }
            }
        }

        public AfterRenderAction JoiningAction
        {
            get => _ara;
            set => SetProperty(ref _ara, value);
        }

        public Renderer Renderer
        {
            get => _renderer;
            set => SetProperty(ref _renderer, value);
        }

        public string OutputPath
        {
            get => _output;
            set => SetProperty(ref _output, value);
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

        public int TotalFrames => Data.End - Data.Start + 1;

        public float RenderProgress
        {
            get => _renderProgress;
            set => SetProperty(ref _renderProgress, value);
        }

        public string StatusTime
        {
            get => _statusTime;
            set => SetProperty(ref _statusTime, value);
        }

        public bool StatusTimeVisible => IsBusy && StatusTime != null;



        public void OpenDonationPage()
        {
            string business = "9SGQVK6TK2UJG";
            string description = "Donation%20for%20Blender%20Render%20Controller";
            string country = "BR";
            string currency = "USD";

            string url = "https://www.paypal.com/cgi-bin/webscr" +
                    "?cmd=_donations" +
                    "&business=" + business +
                    "&lc=" + country +
                    "&item_name=" + description +
                    "&item_number=BRC" +
                    "&currency_code=" + currency +
                    "&bn=PP%2dDonationsBF";

            ShellOpen(url);
        }

        public bool OpenOutputFolder()
        {
            if (Directory.Exists(OutputPath))
            {
                ShellOpen(OutputPath);
                return true;
            }

            return false;
        }

        public void SetBlendData(string filepath, BlendData data)
        {
            BlendFile = filepath;
            Data = data;
        }

        void OnDataUpdated()
        {
            _bkpRange = new Chunk(Data.Start, Data.End);

            AutoFrameRange =
            AutoChunkSize =
            AutoMaxProcessors = true;
        }

        private void RenderManager_AfterRenderStarted(object sender, AfterRenderAction e)
        {
            throw new NotImplementedException();
        }

        private void RenderManager_ProgressChanged(object sender, RenderProgressInfo e)
        {
            RenderProgress = e.FramesRendered / (float)Chunks.TotalLength();
            Footer = $"Completed {e.PartsCompleted} / {Chunks.Count} chunks, " +
                $"{e.FramesRendered} frames rendered";

            _etaCalc.Update(RenderProgress);

            if (_etaCalc.ETAIsAvailable)
            {
                var etr = $"ETR: {_etaCalc.ETR:hh\\:mm\\:ss}";
                StatusTime = etr;
            }
        }

        private void RenderManager_Finished(object sender, BrcRenderResult e)
        {
            throw new NotImplementedException();
        }

        RenderManager _renderMngr;
        ETACalculator _etaCalc;


        int _startFrame, _endFrame, _chunkSize, _maxProcs;
        AfterRenderAction _ara;
        Renderer _renderer;
        bool _autoFrameRange, _autoChunkSize, _autoMaxProcs, _configOK;
        string _output, _statusTime;
        BlendData _data;
        Chunk _bkpRange;
        private float _renderProgress;
    }
}
