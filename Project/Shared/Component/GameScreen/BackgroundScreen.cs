using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Model;
using Model.Base;
using Platman.Component.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platman.Component.GameScreen.Screens
{
    public class BackgroundScreen : AnimatedComponent
    {
        public new Rectangle Frame;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            if (time > 20)
            {
                Frame.X += 10;
                time = 0;
            }

        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin(transformMatrix: Camera.Matrix, samplerState: SamplerState.LinearWrap);

            spriteBatch.Draw(Texture, Position, Frame, Color.White * Alpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            spriteBatch.End();
        }


        private static AnimationManager LoadAnimation()
        {

            return new AnimationManager(new AnimationModel("key1", "Textures/Screen/background", 1,1,1, 100, true));
        }

        private static BackgroundScreen _instance;

        public static BackgroundScreen Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BackgroundScreen();
                else if (!GameRoot.Instance.Components.Contains(_instance))
                    GameRoot.Instance.Components.Add(_instance);

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        private BackgroundScreen() : base(new CompModel(Vector2.Zero), LoadAnimation(), camera: CamManager.Instance.ScreenCamera)
        {
            DrawOrder = 0;
            Frame = base.Frame;
        }
    }
}
