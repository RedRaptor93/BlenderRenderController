using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using BRClib.Commands;
using MvvmHelpers;
using static BRClib.Global;
using BRCRes = BRClib.Properties.Resources;

namespace BRClib.ViewModels
{
    public class BrcMainViewModel : BaseViewModel
    {
        public BrcMainViewModel()
        {
            Title = BRCRes.AppTitle;
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

        // -1 == Indeterminate state
        public float Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
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

        public async Task<MixdownCmd> RunMixdown()
        {
            IsBusy = true;
            Footer = "Rendering mixdown...";

            var tcs = new CancellationTokenSource();
            var mix = new MixdownCmd
            {
                BlendFile = BlendFile,
                Range = new Chunk(StartFrame, EndFrame),
                OutputFolder = OutputPath
            };

            var rc = await mix.RunAsync(tcs.Token);

            IsBusy = false;

            Footer = rc == 0 ? "Mixdown complete" : "Mixdown failed";

            return mix;
        }

        public void StartRender()
        {
            if (AutoChunkSize)
            {
                Chunks = Chunk.CalcChunks(StartFrame, EndFrame, MaxProcessors).ToList();
            }
            else
            {
                Chunks = Chunk.CalcChunksByLength(StartFrame, EndFrame, ChunkSize).ToList();
            }

            // logger.Info("Chunks: " + string.Join(", ", _vm.Chunks));

            IsBusy = true;

            // render manager setup...
            var rj = new RenderJob
            {
                BlendFile = this.BlendFile,
                AudioCodec = this.Data.AudioCodec,
                ProjectName = this.Data.ProjectName,
                ChunksDir = DefaultChunksDirPath,
                OutputPath = this.OutputPath,
                MaxProcessors = this.MaxProcessors,
                Chunks = this.Chunks,
                Duration = this.Duration
            };

            _renderMngr.Setup(rj);

            Progress = 0;
            Footer = "Starting Render...";
            _renderMngr.StartAsync();
        }

        public void StopRender()
        {
            if (_renderMngr.InProgress)
            {
                _renderMngr.Abort();
            }

            _etaCalc.Reset();
            IsBusy = false;
            Progress = 0;
            Title = BRCRes.AppTitle;
        }

        public Action<BrcRenderResult> OnRenderFinished { get; set; }

        public async Task<(int, GetInfoCmd)> OpenBlend(string filepath)
        {
            Footer = "Loading .blend file";
            Progress = -1;
            IsBusy = true;

            if (!File.Exists(filepath))
            {
                return (-1, null);
            }

            var getinfocmd = new GetInfoCmd(filepath);
            await getinfocmd.RunAsync();

            if (getinfocmd.StdOutput.Length == 0)
            {
                return (-2, getinfocmd);
            }

            var data = BlendData.FromPyOutput(getinfocmd.StdOutput);

            short retCode = 0;

            if (RenderFormats.IMAGES.Contains(data.FileFormat))
            {
                retCode = 1;
            }

            if (string.IsNullOrWhiteSpace(data.OutputPath))
            {
                retCode = 2;
                data.OutputPath = Path.GetDirectoryName(filepath);
            }
            else
            {
                data.OutputPath = Path.GetDirectoryName(data.OutputPath);
            }

            Data = data;
            BlendFile = filepath;

            Progress = 0;
            IsBusy = false;
            Footer = "Ready";

            return (retCode, getinfocmd);
        }


        void OnDataUpdated()
        {
            _bkpRange = new Chunk(Data.Start, Data.End);

            OutputPath = Data.OutputPath;

            AutoFrameRange =
            AutoChunkSize =
            AutoMaxProcessors = true;
        }

        private void RenderManager_AfterRenderStarted(object sender, AfterRenderAction e)
        {
            Progress = -1;

            switch (e)
            {
                case AfterRenderAction.MIX_JOIN:
                    Footer = "Joining chunks w/ custom mixdown";
                    break;
                case AfterRenderAction.JOIN:
                    Footer = "Joining chunks";
                    break;
                case AfterRenderAction.MIXDOWN:
                    Footer = "Rendering mixdown";
                    break;
                default:
                    break;
            }
        }

        private void RenderManager_ProgressChanged(object sender, RenderProgressInfo e)
        {
            Progress = e.FramesRendered / (float)TotalFrames;
            Footer = $"Completed {e.PartsCompleted} / {Chunks.Count} chunks, " +
                $"{e.FramesRendered} frames rendered";

            _etaCalc.Update(Progress);

            if (_etaCalc.ETAIsAvailable)
            {
                var etr = $"ETR: {_etaCalc.ETR:hh\\:mm\\:ss}";
                StatusTime = etr;
            }
        }

        private void RenderManager_Finished(object sender, BrcRenderResult e)
        {
            StopRender();
            OnRenderFinished(e);
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
        float _progress;
    }
}
