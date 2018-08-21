using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Model.Screen;
using Platman.Component.Audio;
using Platman.Component.Base;
using Platman.Component.Effect;
using Platman.Component.GameScreen.Screens.Interface;
using Platman.Component.Input;
using Platman.Component.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platman.Component.Gameplay.Map
{
    public class Poetry : DrawableBase
    {
        private Texture2D texture;
        private GameText[] texts = new GameText[3];

        private FadeEffect fadeEffect;

        private string[] verses1;
        private string[] verses2;

        Level level;
        public Poetry(string text, Level level) : base(null)
        {
            this.level = level;

            Camera = CamManager.Instance.CreateCameraIntance(this);
            texture = ContentLoader.Instance.LoadTextureByPath("Textures/Gameplay/Level/poetry");
            Position = new Vector2(255, 50);
            DrawOrder = (int)GameDrawOrder.Poetry;

            string[] parts = text.Split('|');

            // Pegar os versos da segunda parte
            verses1 = GetVerses(parts[0]);
            verses2 = parts.Length > 1 ? GetVerses(parts[1]) : GetVerses("");

            texts[0] = new GameText(new TextModel(new Vector2(450, 200), verses1[0], "Middle"));
            texts[1] = new GameText(new TextModel(new Vector2(357, 240), "", "Small"));
            texts[2] = new GameText(new TextModel(new Vector2(1050, 144), "", "Small"));

            for (int i = 1; i < verses1.Length; i++)
                texts[1].Text += verses1[i];

            for (int i = 0; i < verses2.Length; i++)
                texts[2].Text += verses2[i];

            fadeEffect = new FadeEffect(false, 100, 1f, 0f);
            fadeEffect.IsFocus = true;

            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].Alpha = fadeEffect.Alpha;
                texts[i].Camera = Camera;
                texts[i].DrawOrder = DrawOrder;
            }

            Microsoft.Xna.Framework.Media.MediaPlayer.Volume = 0.1f;

            Game.Components.Add(this);
        }

        private string[] GetVerses(string text)
        {
            var array = text.Split('-');

            // Contar a quantidade de parágrafo e depois colocar na lista string

            int count = 0;

            for (int i = 0; i < text.Length; i++)
            {
                if (text.Substring(i, 1) == "-")
                    count++;
            }

            List<string> list = new List<string>();

            for (int i = 0; i < array.Length; i++)
            {
                list.Add(array[i]);

                if (i < count)
                {
                    list.Add(new StringBuilder().AppendLine().ToString());
                    list.Add(new StringBuilder().AppendLine().ToString());
                }
            }

            return list.ToArray();
        }



        public override void Update(GameTime gameTime)
        {
            fadeEffect.RunEffect(gameTime);
            Alpha = fadeEffect.Alpha;

            level.Alpha = 1f - fadeEffect.Alpha;

            if (level.Alpha < 0.5f)
            {
                level.Alpha = 0.5f;
            }

            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].Alpha = fadeEffect.Alpha;
            }

            if (!fadeEffect.Enabled)
            {
                if (PlayerInput.Instance.IsKeyDown(GameKey.A))
                {
                    level.Alpha += 0.1f;
                    fadeEffect.IsFocus = false;
                    fadeEffect.Finish += FadeEffect_Finish;
                }
            }

        }

        private void FadeEffect_Finish(object sender, System.EventArgs e)
        {
            for (int i = 0; i < texts.Length; i++)
                texts[i].Unload();


            Unload();

            level.Enabled = true;

            Microsoft.Xna.Framework.Media.MediaPlayer.Volume = GameMusicPlayer.Instance.VolumeMusic / 10f;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin(transformMatrix: Camera.Matrix);

            spriteBatch.Draw(texture, Position, Color.White * Alpha);

            spriteBatch.End();
        }
    }

}
