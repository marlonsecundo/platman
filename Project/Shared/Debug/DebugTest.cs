using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platman.Debug;
using System.Collections.Generic;

namespace Platman
{
    public class DebugTest : DrawableGameComponent
    {
        private static DebugTest _instance;
        public static DebugTest Instance => _instance = _instance ?? new DebugTest();

        List<string> texts = new List<string>();
        List<DebugText> texts1 = new List<DebugText>();

        SpriteFont font;
        SpriteBatch spriteBatch;

        Vector2 pos = new Vector2(30, 30);
        Vector2 pos1 = new Vector2(570, 30);
        int time = 0;

        string log = "";
        private DebugTest() : base(GameRoot.Instance)
        {
            UpdateOrder = int.MaxValue;
            DrawOrder = int.MaxValue;

            Game.Components.Add(this);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            font = ContentLoader.Instance.LoadFont(Component.GameScreen.Screens.Interface.TextSize.Small);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void WriteLine(string s)
        {
            if (!texts.Contains(s))
                texts.Add(s);
        }

        public void WriteLine(string s, int timeout)
        {
            texts1.Add(new DebugText(s, timeout));
            log += s + "\n";
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();

            // text
            {
                for (int i = 0; i < texts.Count; i++)
                {
                    spriteBatch.DrawString(font, texts[i], pos, Color.Red);
                    pos.Y += 30;
                }
            }

            // texts1
            {
                for (int i = 0; i < texts1.Count; i++)
                {
                    texts1[i].time += gameTime.ElapsedGameTime.Milliseconds;
                    
                        
                        spriteBatch.DrawString(font, texts1[i].Text, pos1, Color.Red);
                        pos1.Y += 30;
                    
                }
            }
            
            spriteBatch.End();

            // text
            {
                pos = new Vector2(30, 30);
                texts.Clear();
            }

            //texts1
            {
                pos1 = new Vector2(570, 30);


                for (int i = 0; i < texts1.Count; i++)
                {
                    if (texts1[i].time > texts1[i].Timeout)
                    {

                        texts1.Remove(texts1[i]);
                        --i;
                    }
                }


            }
        }


    }
}
