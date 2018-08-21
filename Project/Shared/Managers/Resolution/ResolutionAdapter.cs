using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platman.Extensions;
using System.Diagnostics;

namespace Platman.Managers.Resolution
{
    public class ResolutionAdapter : IResolution
	{
		#region Fields

		/// <summary>
		/// The title safe area for our virtual resolution
		/// </summary>
		/// <value>The title safe area.</value>
		private Rectangle _titleSafeArea;

		/// <summary>
		/// This will be a rectangle of the whole screen in our "virtual resolution"
		/// </summary>
		private Rectangle _screenArea;

		/// <summary>
		/// The actual screen resolution
		/// </summary>
		private Point _screenResolution = new Point(1280, 720);

		/// <summary>
		/// The screen rect we want for our game, and are going to fake
		/// </summary>
		private Point _virtualResolution = new Point(1280, 720);

		/// <summary>
		/// The scale matrix from the desired rect to the screen rect
		/// </summary>
		private Matrix _scaleMatrix;

		/// <summary>
		/// Scale matrix used to convert screen coords (mouse click, touch events) to game coords.
		/// </summary>
		private Matrix _screenMatrix;

		/// <summary>
		/// whether or not we want full screen 
		/// </summary>
		private bool _fullScreen;

		/// <summary>
		/// whether or not the matrix needs to be recreated
		/// </summary>
		private bool _dirtyMatrix = true;

		private bool _letterBox;

		private Vector2 _pillarBox;

		/// <summary>
		/// The graphics device
		/// </summary>
		/// <value>The device.</value>
		private GraphicsDeviceManager DeviceGraphs { get; set; }

		#endregion //Fields

		#region Properties

		public Rectangle TitleSafeArea
		{
			get { return _titleSafeArea; }
		}

		public Rectangle ScreenArea
		{
			get { return _screenArea; }
		}

		public Matrix ScreenMatrix
		{
			get
			{
				return _screenMatrix;
			}
		}

		/// <summary>
		/// Get virtual Mode Aspect Ratio
		/// </summary>
		/// <returns>aspect ratio</returns>
		private float VirtualAspectRatio
		{
			get
			{
				return _virtualResolution.X / (float)_virtualResolution.Y;
			}
		}

		/// <summary>
		/// Get virtual Mode Aspect Ratio
		/// </summary>
		/// <returns>aspect ratio</returns>
		private float ScreenAspectRatio
		{
			get
			{
				return _screenResolution.X / (float)_screenResolution.Y;
			}
		}

		public Point VirtualResolution
		{
			get
			{
				return _virtualResolution;
			}
			set
			{
				SetVirtualResolution(value.X, value.Y);
			}
		}

		public Point ScreenResolution
		{
			get
			{
				return _screenResolution;
			}
			set
			{
				SetScreenResolution(value.X, value.Y, _fullScreen, _letterBox);
			}
		}

		#endregion //Properties

		#region Methods

		#region Initialization

		/// <summary>
		/// default constructor for testing
		/// </summary>
		public ResolutionAdapter()
		{
		}

		/// <summary>
		/// Init the specified device.
		/// </summary>
		/// <param name="deviceMananger">Device.</param>
		public ResolutionAdapter(GraphicsDeviceManager deviceMananger)
		{
			DeviceGraphs = deviceMananger;
			_screenResolution.X = DeviceGraphs.PreferredBackBufferWidth;
			_screenResolution.Y = DeviceGraphs.PreferredBackBufferHeight;
		}

		/// <summary>
		/// The the resolution our game is designed to run in.
		/// </summary>
		/// <param name="Width">Width.</param>
		/// <param name="Height">Height.</param>
		public void SetVirtualResolution(int Width, int Height)
		{
			_virtualResolution = new Point(Width, Height);

			_screenArea = new Rectangle(0, 0, _virtualResolution.X, _virtualResolution.Y);

			//set up the title safe area
			_titleSafeArea.X = (int)(_virtualResolution.X / 20.0f);
			_titleSafeArea.Y = (int)(_virtualResolution.Y / 20.0f);
			_titleSafeArea.Width = (int)(_virtualResolution.X - (2.0f * TitleSafeArea.X));
			_titleSafeArea.Height = (int)(_virtualResolution.Y - (2.0f * TitleSafeArea.Y));

			_dirtyMatrix = true;
		}

		/// <summary>
		/// Sets the screen we are going to use for the screen
		/// </summary>
		/// <param name="Width">Width.</param>
		/// <param name="Height">Height.</param>
		/// <param name="FullScreen">If set to <c>true</c> full screen.</param>
		public void SetScreenResolution(int Width, int Height, bool FullScreen, bool letterbox)
		{
			_screenResolution.X = Width;
			_screenResolution.Y = Height;
			_letterBox = letterbox;

            switch (Device.Instance.DeviceType)
            {
                case DType.Android:
                case DType.IOS:
                case DType.Win10_Phone:
                    _fullScreen = true;
                    break;
                default:
                    _fullScreen = FullScreen;
                    break;
            }


            ApplyResolutionSettings();

           
        }

        protected virtual void ApplyResolutionSettings()
        {
            if (!_fullScreen)
            {
                //Make sure the width isn't bigger than the screen
                if (_screenResolution.X > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                {
                    _screenResolution.X = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                }

                //Make sure the height isn't bigger than the screen
                if (_screenResolution.Y > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
                {
                    _screenResolution.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                }
            }
            else
            {
                bool bFound = false;
                foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    // Check the width and height of each mode against the passed values
                    if ((dm.Width == _screenResolution.X) && (dm.Height == _screenResolution.Y))
                    {
                        // The mode is supported, so set the buffer formats, apply changes and return
                        bFound = true;
                        break;
                    }
                }
                if (!bFound)
                {
                    switch (Device.Instance.DeviceType)
                    {
                        case DType.Android:
                        case DType.IOS:
                        case DType.Win10_Phone:
                            break;
                        default:
                            _screenResolution.X = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                            _screenResolution.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                            break;
                    }
                }

            }



            //ok, found a good set of stuff... set the graphics device
            DeviceGraphs.PreferredBackBufferWidth = _screenResolution.X;
            DeviceGraphs.PreferredBackBufferHeight = _screenResolution.Y;
            DeviceGraphs.IsFullScreen = _fullScreen;
            DeviceGraphs.ApplyChanges();


            //Update the virtual resolution to match the aspect ratio of the new actual resolution
            if (!_letterBox)
            {
                UpdateVirtualResolution();
            }

            //we are gonna have to redo that scale matrix
            _dirtyMatrix = true;


       

        }

        protected void UpdateVirtualResolution()
		{
			if (ScreenAspectRatio < VirtualAspectRatio)
			{
				//the width needs to be pulled in to match the screen aspect ratio
				var width = ((_screenResolution.X * _virtualResolution.Y) / _screenResolution.Y);
				SetVirtualResolution(width, _virtualResolution.Y);
			}
			else if (ScreenAspectRatio > VirtualAspectRatio)
			{
				//the height needs to be pulled in to match the screen aspect ratio
				var height = ((_virtualResolution.X * _screenResolution.Y) / _screenResolution.X);
				SetVirtualResolution(_virtualResolution.X, height);
			}
		}

		#endregion Initialization

		public Matrix TransformationMatrix()
		{
			if (_dirtyMatrix)
			{
				RecreateScaleMatrix(new Point(
					DeviceGraphs.GraphicsDevice.Viewport.Width,
					DeviceGraphs.GraphicsDevice.Viewport.Height));
			}

			return _scaleMatrix;
		}

		public Vector2 ScreenToGameCoord(Vector2 screenCoord)
		{
            return _screenMatrix.Multiply(screenCoord);
		}

		protected virtual void RecreateScaleMatrix(Point vp)
		{
            _dirtyMatrix = false;
			_scaleMatrix = Matrix.CreateScale(
                vp.X / (float)_virtualResolution.X,
                vp.Y / (float)_virtualResolution.Y,
				1.0f);

            var translation = Matrix.CreateTranslation(_pillarBox.X, _pillarBox.Y, 0f);

			_screenMatrix = Matrix.Multiply(translation, Matrix.CreateScale(
                _virtualResolution.X / (float)vp.X,
                _virtualResolution.Y / (float)vp.Y,
				1.0f));
		}

		public void ResetViewport()
		{
			int width = DeviceGraphs.GraphicsDevice.Viewport.Width;
			var height = (int)(width / VirtualAspectRatio + .5f);
			bool changed = false;

			if (height != DeviceGraphs.GraphicsDevice.Viewport.Height || width != DeviceGraphs.GraphicsDevice.Viewport.Width)
			{
				if (height > DeviceGraphs.GraphicsDevice.Viewport.Height)
				{
					height = DeviceGraphs.PreferredBackBufferHeight;
					width = (int)(height * VirtualAspectRatio + .5f);
					changed = true;
				}

				var viewport = new Viewport()
				{
					X = (DeviceGraphs.PreferredBackBufferWidth / 2) - (width / 2),
					Y = (DeviceGraphs.PreferredBackBufferHeight / 2) - (height / 2),
					Width = width,
					Height = height,
					MinDepth = 0,
					MaxDepth = 1
				};

				_pillarBox = new Vector2(-viewport.X, -viewport.Y);

				if (changed)
				{
					_dirtyMatrix = true;
				}

				DeviceGraphs.GraphicsDevice.Viewport = viewport;

          
            }
    
        }

		#endregion
	}
}
