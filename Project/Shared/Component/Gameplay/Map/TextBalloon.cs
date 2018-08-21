using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Platman.Component.Audio;
using Platman.Component.Base;
using Platman.Component.Effect;
using Platman.Component.Gameplay.Person;
using Platman.Component.Gameplay.Mechanic.Gravity;
using System;
using VelcroPhysics.Dynamics;

namespace Platman.Component.Gameplay.Map
{
    public sealed class TextBalloon : DrawableBase
    {
        public event EventHandler Finished;

        FadeEffect fadeText;
        FadeEffect fadeBallon;

        Texture2D textureStart;
        Texture2D textureEnd;
        Texture2D textureWidth;

        Hero hero;
        SpriteFont font;
        string Text { get; set; }


        string[] pharases;
        int time;
        int textIndex;

        Vector2 startPos;
        Vector2 middlePos;
        Vector2 endPos;
        Vector2 textPos;

        Rectangle ballonBounds;

        SoundEffect textSound;
        World world;

        public TextBalloon(string text, Hero hero, World world) : base(hero.Camera)
        {
            this.world = world;
            textSound = ContentLoader.Instance.LoadSound("talk");

            Finished += TextBalloon_Finished;

            fadeText = new FadeEffect(false, 50, 1f, 0f);
            fadeBallon = new FadeEffect(false, 70, 1f, 0f);

            textureStart = ContentLoader.Instance.LoadTextureByPath("Textures/Gameplay/ballonStart");
            textureWidth = ContentLoader.Instance.LoadTextureByPath("Textures/Gameplay/ballonWidth");
            textureEnd = ContentLoader.Instance.LoadTextureByPath("Textures/Gameplay/ballonEnd");

            this.hero = hero;
            font = ContentLoader.Instance.LoadFont(GameScreen.Screens.Interface.TextSize.Small);
            pharases = text.Split('-');
            Text = pharases[0];

            textIndex = 0;

            fadeBallon.IsFocus = true;
            fadeBallon.Finish += FadeBallon_Completed;

            fadeText.IsFocus = true;
            fadeText.Finish += FadeEffect_Completed;

            SetBallonBounds(Text);

            Game.Components.Add(this);

            DrawOrder = (int)GameDrawOrder.Input;

        }

        private void TextBalloon_Finished(object sender, EventArgs e)
        {

        }


        private void FadeBallon_Completed(object sender, EventArgs e)
        {
            if (!fadeBallon.IsFocus)
            {
                Finished(this, null);
                Unload();
            }
        }

        private void SetBallonBounds(string text)
        {
            Vector2 value = font.MeasureString(text);
            int width = (int)Math.Round(value.X);
            int height = (int)Math.Round(value.Y);

            ballonBounds = new Rectangle(0, 0, width, textureWidth.Height);
            GameSoundPlayer.Instance.PlaySound(textSound);
        }

        private void FadeEffect_Completed(object sender, EventArgs e)
        {
            if (!fadeText.IsFocus)
            {
                textIndex++;

                if (textIndex < pharases.Length)
                {
                    Text = pharases[textIndex];
                    fadeText.IsFocus = true;
                    SetBallonBounds(Text);
                }
                else
                {
                    fadeBallon.IsFocus = false;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (fadeText.IsFocus && !fadeText.Enabled)
            {
                time += gameTime.ElapsedGameTime.Milliseconds;

                if (time >= 800)
                {
                    time = 0;
                    fadeText.IsFocus = false;
                }
            }

            fadeText.RunEffect(gameTime);
            fadeBallon.RunEffect(gameTime);


            UpdatePosition(gameTime);
        }

        private void UpdatePosition(GameTime gameTime)
        {
            if (world.Gravity.Y > 0) startPos = new Vector2(hero.DrawBounds.Right, hero.DrawBounds.Top - textureStart.Height);
            else if (world.Gravity.Y < 0) startPos = new Vector2(hero.DrawBounds.Right, hero.DrawBounds.Bottom);
            else if (world.Gravity.X < 0) startPos = new Vector2(hero.DrawBounds.Right, hero.DrawBounds.Top + hero.DrawBounds.Height);
            else if (world.Gravity.X > 0) startPos = new Vector2(hero.DrawBounds.Left - textureStart.Width - ballonBounds.Width - textureEnd.Width, hero.DrawBounds.Bottom + 10);

            Vector2 space = new Vector2(10, 10);
            textPos = startPos + space;

            middlePos = startPos;
            middlePos.X += textureStart.Width;

            endPos = middlePos;
            endPos.X += ballonBounds.Width;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(transformMatrix: Camera.Matrix, samplerState: SamplerState.LinearClamp);

            spriteBatch.Draw(textureStart, startPos, Color.White * fadeBallon.Alpha);
            spriteBatch.Draw(textureWidth, middlePos, ballonBounds, Color.White * fadeBallon.Alpha);
            spriteBatch.Draw(textureEnd, endPos, Color.White * fadeBallon.Alpha);
            spriteBatch.DrawString(font, Text, textPos, Color.White * fadeText.Alpha);
            spriteBatch.End();
        }



    }
}
