using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platman.Component.Audio;
using Platman.Component.Input;
using Platman.Component.Managers;
using Platman.DataBase;

namespace Platman
{
    public partial class GameRoot : Game
    {
        public GraphicsDeviceManager Graphics { get; }
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        public Point Display{ get; }
        public GameRoot()
        {
            _instance = this;
            Content.RootDirectory = "Content";

            Graphics = new GraphicsDeviceManager(this);
            Graphics.ApplyChanges();
        }


        protected override void Initialize()
        {
            object init = Settings.Instance;
            init = ResolutionManager.Instance;
            init = ScreenManager.Instance;
            init = PlayerInput.Instance;
            init = DebugTest.Instance;
            init = null;

            CamManager.Instance.RecalculateTransformationMatrices();
            
            base.Initialize();

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("pinguin");            

            GameMusicPlayer.Instance.PlayScreenMusic();
        }

        int time = 0;
        bool b = false;

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
     

            

        }


        protected override void Draw(GameTime gameTime)
        {
            Graphics.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
/*
            spriteBatch.Begin(SpriteSortMode.Immediate,
                                  BlendState.AlphaBlend,
                                  null, null, null, null,
                                  CamManager.Instance.ScreenCamera.Matrix);

            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
            
            spriteBatch.End();*/
        }

        
    }

    public partial class GameRoot : Game
    {
        private static GameRoot _instance;

        public static GameRoot Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
