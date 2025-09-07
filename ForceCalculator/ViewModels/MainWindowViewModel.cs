using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using ForceCalculationLib;
using ForceCalculationLib.RobotsBuilders;
using ForceCalculationLib.Solvers;
using ForceCalculationLib.SupportsBuilders;
using ForceCalculator.Extensions;

namespace ForceCalculator.ViewModels
{
	public partial class MainWindowViewModel : ViewModelBase
	{
		private int _supportsBuilderType = 0;
		public int SupportsBuilderType
		{
			get
			{
				return _supportsBuilderType;
			}
			set
			{
				SetProperty(ref _supportsBuilderType, value);

				switch(value)
				{
					case 0:
						_supportsBuilder = new AllRandomSupportsBuilder();
						break;
					case 1:
						_supportsBuilder = new TwoLineSupportsBuilder();
						break;
					case 2:
						_supportsBuilder = new CircleSuportsBuilder();
						break;
					default:
						throw new NotImplementedException();
				}

				if(IsSaveEnabled)
					DrawPlatform();
			}
		}

		private ISupportsBuilder _supportsBuilder = new AllRandomSupportsBuilder();

		private int _robotsBuilderType = 0;
		public int RobotsBuilderType
		{
			get
			{
				return _robotsBuilderType;
			}
			set
			{
				SetProperty(ref _robotsBuilderType, value);

				switch(value)
				{
					case 0:
						_robotsBuilder = new AllRandomRobotsBuilder();
						break;
					case 1:
						_robotsBuilder = new OneLineRobotsBuilder();
						break;
					case 2:
						_robotsBuilder = new TwoLineRobotsBuilder();
						break;
					case 3:
						_robotsBuilder = new PerimeterRobotsBuilder();
						break;
					case 4:
						_robotsBuilder = new CircleRobotsBuilder();
						break;
					default:
						throw new NotImplementedException();
				}

				if(IsSaveEnabled)
					DrawPlatform();
			}
		}

		private IRobotsBuilder _robotsBuilder = new AllRandomRobotsBuilder();

		private double _supportNumber = 3;
		public double SupportNumber
		{
			get
			{
				return _supportNumber;
			}
			set
			{
				SetProperty(ref _supportNumber, Math.Round(value));
				if(IsSaveEnabled)
					DrawPlatform();
			}
		}

		private double _robotNumber = 2;
		public double RobotNumber
		{
			get
			{
				return _robotNumber;
			}
			set
			{
				SetProperty(ref _robotNumber, Math.Round(value));
				if(IsSaveEnabled)
					DrawPlatform();
			}
		}

		private float _gravityForce = -1000f;
		public float GravityForce
		{
			get => _gravityForce;
			set => SetProperty(ref _gravityForce, value);
		}

		private float _robotsForce;
		public float RobotsForce
		{
			get => _robotsForce;
			set => SetProperty(ref _robotsForce, value);
		}

		private float _simplexRobotForce;
		public float SimplexRobotsForce
		{
			get => _simplexRobotForce;
			set => SetProperty(ref _simplexRobotForce, value);
		}

		private bool _isValid;
		public bool IsValid
		{
			get => _isValid;
			set => SetProperty(ref _isValid, value);
		}

		private bool _simplexIsValid;
		public bool SimplexIsValid
		{
			get => _simplexIsValid;
			set => SetProperty(ref _simplexIsValid, value);
		}

		private RenderTargetBitmap? _imageBitmap;
		public RenderTargetBitmap? ImageBitmap
		{
			get => _imageBitmap;
			set => SetProperty(ref _imageBitmap, value);
		}

		public bool IsSaveEnabled { get => ImageBitmap != null; }

		private int _seed;


		[RelayCommand]
		private void UpdateImage()
		{
			_seed = DateTime.Now.Microsecond;
			DrawPlatform();
			OnPropertyChanged(nameof(IsSaveEnabled));
		}

		[RelayCommand]
		private void SaveImage()
		{
			if(!Directory.Exists("images"))
				Directory.CreateDirectory("images");

			ImageBitmap?.Save($"images\\image_{DateTime.Now.Ticks}.png");
		}

		[RelayCommand]
		private void OpenImageDirectory()
		{
			if(!Directory.Exists("images"))
				Directory.CreateDirectory("images");

			using Process fileOpener = new Process();
			fileOpener.StartInfo.FileName = "explorer";
			fileOpener.StartInfo.Arguments = ".\\images";
			fileOpener.Start();
			return;
		}



		private void DrawPlatform()
		{
			Supports supports = _supportsBuilder.Build((int)SupportNumber, _seed);
			Robot[] robots = _robotsBuilder.Build(supports, (int)RobotNumber, ~_seed);

			Platform platform = new Platform(supports, robots, GravityForce);
			ImageBitmap = platform.ToBitmap(1024);

			ISolver solver = new EqualMaxSolver();
			solver.Solve(platform);
			RobotsForce = platform.Robots.Select(r => r.PushingForce).Aggregate((a, v) => a + v);
			IsValid = SkewSolver.IsPlatformValid(platform);

			SimplexSolver simplexSolver = new();
			simplexSolver.Solve(platform);
			SimplexRobotsForce = platform.Robots.Select(r => r.PushingForce).Aggregate((a, v) => a + v);
			SimplexIsValid = SkewSolver.IsPlatformValid(platform);
		}
	}
}
