using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using BRClib.Commands;
using MvvmHelpers;
using static BRClib.Global;
using Res = BRClib.Properties.Resources;

namespace BRClib.ViewModels
{
    public class BrcMainViewModel : BaseViewModel
    {
        public BrcMainViewModel()
        {
            Title = Res.AppTitle;
            Header = "blend name"; // project name
            Footer = "Ready"; // Status
            CanLoadMore = false; // Can load blend

            Chunks = new List<Chunk>();
            _bkpRange = new Chunk(1, 50);
            _data = _NoData;

            _startFrame = _bkpRange.Start;
            _endFrame = _bkpRange.End;
            _maxProcs = Environment.ProcessorCount;
			_ara = Settings.AfterRender;
			_renderer = Settings.Renderer;
			_configOK = false;
            _dataRdy = false;

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
                SetProperty(ref _configOK, value);
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

        public bool ProjectLoaded
		{
			get => _dataRdy;
			private set 
			{ 
				if (SetProperty(ref _dataRdy, value))
				{
					CanLoadMore = ProjectLoaded && IsNotBusy;
				} 
			}
		}

        public string BlendFile { get; private set; }

        public List<Chunk> Chunks { get; set; }

        public string ActiveScene 
        { 
            get => Data.ActiveScene;
            set => Data.ActiveScene = value;
        }

        public TimeSpan Duration
        {
            get
            {
                double d = (StartFrame - EndFrame + 1) / Fps;
                if (!double.IsNaN(d) && !double.IsInfinity(d))
                    return TimeSpan.FromSeconds(d);

                return TimeSpan.Zero;
            }
        }


        public double Fps         
        { 
            get => Data.Fps;
            set => Data.Fps = value;
        }

        public string Resolution 
        { 
            get => Data.Resolution;
            set => Data.Resolution = value;
        }


        public int StartFrame
        {
            get => _startFrame;
            set
            {
                if (SetProperty(ref _startFrame, value) && AutoChunkSize)
                {
                    ChunkSize = -1;
                }
            }
        }

        public int EndFrame
        {
            get => _endFrame;
            set
            {
                if (SetProperty(ref _endFrame, value) && AutoChunkSize)
                {
                    ChunkSize = -1;
                }
            }
        }

        public int ChunkSize
        {
            get => _chunkSize;
            set
            {
                SetProperty(ref _chunkSize, value);
            }
        }

        public int MaxCores
        {
            get => _maxProcs;
            set
            {
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
                        ChunkSize = -1;
                    }
                }
            }
        }

        public bool AutoMaxCores
        {
            get => _autoMaxProcs;
            set
            {
                if (SetProperty(ref _autoMaxProcs, value))
                {
                    if (_autoMaxProcs)
                    {
                        MaxCores = Environment.ProcessorCount;
                        RecalcChunkSize();
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
						ResetFrameRange();
                    }
                }
            }
        }

        public AfterRenderAction JoiningAction
        {
            get => _ara;
			set
			{
				if (SetProperty(ref _ara, value))
				{
					Settings.AfterRender = _ara;
				}
			}
        }

        public Renderer Renderer
        {
            get => _renderer;
            set 
			{
				if (SetProperty(ref _renderer, value))
                {
					Settings.Renderer = _renderer;
                }
            }
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

        // -1f == Indeterminate state
        public float Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        public string StatusTime
        {
            get => "ETR: " + WorkETR.ToString(@"hh\:mm\:ss");
        }

        public TimeSpan WorkETR { get; private set; }


        public void ResetFrameRange()
        {
            StartFrame = _bkpRange.Start;
            EndFrame = _bkpRange.End;
        }

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

        public void OpenReportIssuePage()
        {
            ShellOpen("https://github.com/RedRaptor93/BlenderRenderController/wiki/Reporting-an-issue");
        }

        public void OpenProjectPage()
        {
            ShellOpen("https://github.com/RedRaptor93/BlenderRenderController");
        }

        public void UnloadProject()
		{
			Data = _NoData;
		}

        public async Task<MixdownCmd> RunMixdown()
        {
            IsBusy = true;
            Footer = "Rendering mixdown...";
            Progress = -1;

            ResetCTS();

            var mix = new MixdownCmd
            {
                BlendFile = BlendFile,
                Range = new Chunk(StartFrame, EndFrame),
                OutputFolder = OutputPath
            };

            var rc = await mix.RunAsync(_sharedCTS.Token);

            IsBusy = false;
            Footer = rc == 0 ? "Mixdown complete" : "Mixdown failed";
            Progress = 0;

            return mix;
        }

        public async Task<ConcatCmd> RunConcat(string concatFile, string outputPath, string mixdownFile = null)
        {
            IsBusy = true;
            Footer = "Concatenating...";
            Progress = -1;
            
            var c = new ConcatCmd
            {
                ConcatTextFile = concatFile,
                MixdownFile = mixdownFile,
                OutputFile = outputPath
            };

            var rc = await c.RunAsync();

            IsBusy = false;
            Footer = rc == 0 ? "Concatenation complete" : "Concatenation failed";
            Progress = 0;

            return c;
        }

        public void StartRender()
        {
            IsBusy = true;
            Progress = 0;
            Footer = "Starting Render...";

            if (AutoChunkSize)
            {
                Chunks = Chunk.CalcChunks(StartFrame, EndFrame, MaxCores).ToList();
            }
            else
            {
                Chunks = Chunk.CalcChunksByLength(StartFrame, EndFrame, ChunkSize).ToList();
            }

            // logger.Info("Chunks: " + string.Join(", ", _vm.Chunks));

            // render manager setup...
            var rj = new RenderJob
            {
                BlendFile = this.BlendFile,
                AudioCodec = this.Data.AudioCodec,
                ProjectName = this.Data.ProjectName,
                ChunksDir = DefaultChunksDirPath,
                OutputPath = this.OutputPath,
                MaxCores = this.MaxCores,
                Chunks = this.Chunks,
                Duration = this.Duration
            };

            _renderMngr.Setup(rj);
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
            Title = Res.AppTitle;
            Footer = "Ready";
        }

        public Action<BrcRenderResult> OnRenderFinished { get; set; }

        public async Task<(int, GetInfoCmd)> OpenBlend(string filepath)
        {
            Footer = "Loading .blend file";
            Progress = -1;
            IsBusy = true;

            if (!File.Exists(filepath))
            {
                Footer = Res.G_error + ": " + Res.G_file_not_found;
                Progress = 0;
                IsBusy = false;
                return (-1, null);
            }

            var getinfocmd = new GetInfoCmd(filepath);
            await getinfocmd.RunAsync();

            if (getinfocmd.StdOutput.Length == 0)
            {
                Footer = Res.G_error + ": No info";
                Progress = 0;
                IsBusy = false;
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

        public void CancelExtraTasks()
        {
            if (_sharedCTS != null)
            {
                _sharedCTS.Cancel();
            }
        }


        void OnDataUpdated()
        {
            ProjectLoaded = Data != _NoData;

            _bkpRange = ProjectLoaded ? new Chunk(Data.Start, Data.End) : new Chunk(1,50);

            // sync changes
            OutputPath = Data.OutputPath;
            Header = Data.ProjectName;
			ResetFrameRange();

            AutoFrameRange =
            AutoChunkSize =
            AutoMaxCores = true;

			// send events to update infobox items
			OnPropertyChanged(nameof(ActiveScene));
			OnPropertyChanged(nameof(Duration));
			OnPropertyChanged(nameof(Fps));
			OnPropertyChanged(nameof(Resolution));
        }

        void RecalcChunkSize()
        {
            ChunkSize = (int)Math.Ceiling(EndFrame - StartFrame + 1 / (double)MaxCores);
        }

        void ResetCTS()
        {
            if (_sharedCTS != null)
            {
                _sharedCTS.Dispose();
                _sharedCTS = null;
            }
            _sharedCTS = new CancellationTokenSource();
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
            Progress = e.FramesRendered / (float)(EndFrame - StartFrame + 1);
            Footer = $"Completed {e.PartsCompleted} / {Chunks.Count} chunks, " +
                $"{e.FramesRendered} frames rendered";

            _etaCalc.Update(Progress);

            if (_etaCalc.ETAIsAvailable)
            {
                WorkETR = _etaCalc.ETR;
            }
        }

        private void RenderManager_Finished(object sender, BrcRenderResult e)
        {
            OnRenderFinished(e);
            StopRender();
        }


        RenderManager _renderMngr;
        ETACalculator _etaCalc;


        int _startFrame, _endFrame, _chunkSize, _maxProcs;
        AfterRenderAction _ara;
        Renderer _renderer;
        bool _autoFrameRange, _autoChunkSize, _autoMaxProcs, _configOK, _dataRdy;
        string _output;
        BlendData _data;
        Chunk _bkpRange;
        float _progress;
        CancellationTokenSource _sharedCTS;
		static readonly BlendData _NoData = new BlendData();
    }
}
