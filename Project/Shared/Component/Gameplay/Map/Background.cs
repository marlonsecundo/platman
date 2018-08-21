using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Model.Gameplay;
using Platman.Component.Base;
using Platman.Component.GameScreen.Screens.Interface;
using Platman.Component.Managers;
using System.Collections.Generic;

namespace Platman.Component.Gameplay.Map
{
    public class Background : DrawableBase
    {
        private Texture2D texture;

        protected SpriteBatch spritebatch = new SpriteBatch(GameRoot.Instance.GraphicsDevice);

        Art BackgroundArt { get; }

        List<Art> Arts;

        public override float Alpha
        {
            get => base.Alpha;
            set
            {
                base.Alpha = value;

                for (int i = 0; i < AllComps.Count; i++)
                    AllComps[i].Alpha = value;
            }
        }

        public override bool Enabled
        {
            get => base.Enabled;
            set
            {
                base.Enabled = value;

                for (int i = 0; i < AllComps.Count; i++)
                    AllComps[i].Enabled = value;
            }
        }

        public override bool Visible
        {
            get => base.Visible;
            set
            {
                base.Visible = value;

                for (int i = 0; i < AllComps.Count; i++)
                    AllComps[i].Visible = value;
            }
        }

        public override int DrawOrder
        {
            get => base.DrawOrder;
            set
            {
                base.DrawOrder = value;

                for (int i = 0; i < AllComps.Count; i++)
                    AllComps[i].DrawOrder = value;
            }
        }

        public List<Art> AllComps = new List<Art>();

        public Background(BackgroundModel model) : base(null)
        {
            // Camera da imagem de fundo
            Camera = CamManager.Instance.CreateCameraIntance(this);
            CamManager.Instance.LevelCamera.CameraMovedEvent += LevelInstance_MovedEvent;

            BackgroundArt = new Art(model.background);
            BackgroundArt.Camera = Camera;
            // Camera dos componentes da tela de fundo            
            for (int i = 0; i < model.arts.Count; i++)
            {
                Arts[i] = new Art(model.arts[i]);
                Arts[i].Camera = Camera;
            }

            AllComps.Add(BackgroundArt);
            for (int i = 0; i < Arts.Count; i++)
                AllComps.Add(Arts[i]);

            DrawOrder = (int)GameDrawOrder.Background;
            Camera.Position = model.cameraPosition;
        }

        private void LevelInstance_MovedEvent(Vector2 amount)
        {
            Camera.Position += amount;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spritebatch.Begin(transformMatrix: Camera.Matrix, samplerState: SamplerState.LinearWrap);
            spritebatch.Draw(texture, Position, DrawBounds, Color.White * Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spritebatch.End();
        }

    }
}
