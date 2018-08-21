using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace Platman.Component.Base
{
    public enum GameDrawOrder : int
    {
        Background = 0,

        Itens = 2,
        Person = 3,
        Blocks = 4,
        Texts = 5,
        Level = 6,
        Input = 7,
        Poetry = 8

    }

    public delegate void Handler(object sender);

    public abstract class DrawableBase : DrawableGameComponent
    {
        public event Handler Unloaded;

        protected readonly SpriteBatch spriteBatch;
        public virtual Camera2D Camera { get; set; }
        public virtual Vector2 Position { get; set; }
        protected new GameRoot Game { get; }
        public virtual Rectangle DrawBounds { get; set; }
        public virtual float Alpha
        {
            get;
            set;
        }
        public virtual new bool Enabled { get => base.Enabled; set => base.Enabled = value; }
        public virtual new bool Visible { get => base.Visible; set => base.Visible = value; }
        public virtual new int DrawOrder { get => base.DrawOrder; set => base.DrawOrder = value; }


        public DrawableBase(Camera2D camera) : base(GameRoot.Instance)
        {
            Game = GameRoot.Instance;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Camera = camera;
            Unloaded += DrawableBase_Unloaded;
        }

        private void DrawableBase_Unloaded(object sender)
        {

        }

        public virtual void Unload()
        {
            UnloadContent();
            Game.Components.Remove(this);
            Unloaded(this);
        }

    }
}
