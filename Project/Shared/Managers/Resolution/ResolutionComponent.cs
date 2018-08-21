using Microsoft.Xna.Framework;
using System;

namespace Platman.Managers.Resolution
{
    public class ResolutionComponent : DrawableGameComponent, IResolution
    {
        #region Fields

        private Point _virtualResolution;

        private Point _screenResolution;

        private bool _fullscreen;

        private bool _letterbox;

        private readonly GraphicsDeviceManager _graphics;

        #endregion //Fields

        #region Properties

        private ResolutionAdapter ResolutionAdapter { get; set; }

        public Rectangle TitleSafeArea
        {
            get
            {
                return ResolutionAdapter.TitleSafeArea;
            }
        }

        public Rectangle ScreenArea
        {
            get
            {
                return ResolutionAdapter.ScreenArea;
            }
        }

        public Matrix ScreenMatrix =>  ResolutionAdapter.ScreenMatrix;

        public Point VirtualResolution
        {
            get
            {
                return _virtualResolution;
            }
            set
            {
                if (null != ResolutionAdapter)
                {
                    throw new Exception("Can't change VirtualResolution after the ResolutionComponent has been initialized");
                }
                _virtualResolution = value;
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
                if (null != ResolutionAdapter)
                {
                    throw new Exception("Can't change ScreenResolution after the ResolutionComponent has been initialized");
                }
                _screenResolution = value;
            }
        }

        public bool FullScreen
        {
            get
            {
                return _fullscreen;
            }
            set
            {
                if (null != ResolutionAdapter)
                {
                    throw new Exception("Can't change FullScreen after the ResolutionComponent has been initialized");
                }
                _fullscreen = value;
            }
        }

        public bool LetterBox
        {
            get
            {
                return _letterbox;
            }
            set
            {
                if (null != ResolutionAdapter)
                {
                    throw new Exception("Can't change LetterBox after the ResolutionComponent has been initialized");
                }
                _letterbox = value;
            }
        }

        #endregion //Properties

        #region Methods
        
        public ResolutionComponent(Game game, GraphicsDeviceManager graphics, Point virtualResolution, Point screenResolution, bool fullscreen, bool letterbox) : base(game)
        {
            _graphics = graphics;
            VirtualResolution = virtualResolution;
            ScreenResolution = screenResolution;
            _fullscreen = fullscreen;
            _letterbox = letterbox;

            Game.Components.Add(this);

            Game.Services.AddService<IResolution>(this);

            ResolutionAdapter = new ResolutionAdapter(_graphics);
            ResolutionAdapter.SetVirtualResolution(VirtualResolution.X, VirtualResolution.Y);
            ResolutionAdapter.SetScreenResolution(ScreenResolution.X, ScreenResolution.Y, _fullscreen, _letterbox);
            ResolutionAdapter.ResetViewport();

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public Vector2 ScreenToGameCoord(Vector2 screenCoord)
        {
            return ResolutionAdapter.ScreenToGameCoord(screenCoord);
        }

        public Matrix TransformationMatrix()
        {
            return ResolutionAdapter.TransformationMatrix();
        }

        public override void Draw(GameTime gameTime)
        {
            ResolutionAdapter.ResetViewport();
            base.Draw(gameTime);
        }

        public void ResetViewport()
        {
            ResolutionAdapter.ResetViewport();
        }

        #endregion //Methods
    }
}
