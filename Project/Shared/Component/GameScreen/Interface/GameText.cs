using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Model.Screen;
using Platman.Component.Base;
using Platman.Component.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platman.Component.GameScreen.Screens.Interface
{
    public enum TextSize
    {
        Small,
        Middle,
        MiddleSmall,
        Big,
        ExtraBig
    }

    public sealed class GameText : DrawableBase
    {
        public string Text { get; set; }
        public SpriteFont Font { get; }
        public TextSize Size { get; private set; }
        public TextModel Model { get; private set; }
        public Color Foreground { get; set; }

        public override Rectangle DrawBounds
        {
            get
            {
                Vector2 size = Font.MeasureString(Text);
                return new Rectangle((int)Position.X, (int)Position.Y, (int)(size.X), (int)(size.Y));
            }
        }


        public GameText(TextModel model) : base(CamManager.Instance.ScreenCamera)
        {
            TextSize size;
            Enum.TryParse(model.size, out size);

            Size = size;
            Position = model.position;
            Text = model.text;
            Font = ContentLoader.Instance.LoadFont(Size);
            Enabled = false;
            Foreground = Color.White;

            Game.Components.Add(this);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin(transformMatrix: Camera.Matrix);
            spriteBatch.DrawString(Font, Text, Position, Foreground * Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
