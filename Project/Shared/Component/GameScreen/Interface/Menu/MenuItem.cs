using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platman.Component.Base;
using Platman.Component.Effect;
using Platman.Component.Input;
using System.Collections.Generic;

namespace Platman.Component.GameScreen.Screens.Interface.Menu
{
    public delegate void FocusEventHandler(int index, bool sound = true);
    public class MenuItem : GameComponent
    {
        public event FocusEventHandler OnFocus;
        protected SpriteBatch spriteBatch;
        private FadeEffect fadeEffect;

        Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set
            {
                Vector2 amount = Position - value;

                for (int i = 0; i < AllComps.Count; i++)
                    AllComps[i].Position += amount;

                _position = value;
            }
        }
        public bool Visible
        {
            set
            {
                for (int i = 0; i < AllComps.Count; i++)
                    AllComps[i].Visible = value;
            }
        }

        public bool IsFocused
        {
            get { return fadeEffect.IsFocus; }
            set
            {
                fadeEffect.IsFocus = value;
                for (int i = 0; i < AllComps.Count; i++)
                    AllComps[i].Enabled = value;
            }
        }

        public float Alpha
        {
            get { return fadeEffect.Alpha; }
            set
            {
                if (value <= fadeEffect.Alpha)
                {
                    for (int i = 0; i < AllComps.Count; i++)
                        AllComps[i].Alpha = value;
                }
            }
        }

        public int DrawOrder
        {
            set
            {
                for (int i = 0; i < AllComps.Count; i++)
                    AllComps[i].DrawOrder = value;
            }
        }

        public new bool Enabled
        {
            get => base.Enabled;

            set
            {
                base.Enabled = value;
            }
        }

        public int Index { get; set; }
        public List<DrawableBase> AllComps { get; private set; }

        private Handler OnClick;
        private GameKey Key;
        protected int timer = 0;
        private bool hasClickEvent;
        private PlayerInput input;


        public MenuItem(int index, GameKey key, Handler method) : base(GameRoot.Instance)
        {
            _Contructor(index);
            Key = key;
            OnClick = method;
            hasClickEvent = true;
            Game.Components.Add(this);
        }

        public MenuItem(int index) : base(GameRoot.Instance)
        {
            _Contructor(index);
            hasClickEvent = false;
            Game.Components.Add(this);
        }

        private void _Contructor(int index)
        {
            AllComps = new List<DrawableBase>();
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            fadeEffect = new FadeEffect(false, 30, 1.0f, 0.3f);
            Alpha = fadeEffect.MinAlpha;
            Index = index;
        }

        public override void Initialize()
        {
            base.Initialize();
            input = PlayerInput.Instance;

        }

        public void AddComp(DrawableBase comp)
        {
            comp.Enabled = IsFocused;
            comp.Alpha = Alpha;
            AllComps.Add(comp);
        }

        public override void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;

            if (IsMouseOverBounds())
                OnFocus(Index);

            fadeEffect.RunEffect(gameTime);


            if (IsFocused && hasClickEvent)
            {
                if (IsMouseClickComps())
                {
                    OnClick(this);
                }
                else if (input.IsKeyDown(Key))
                {
                    OnClick(this);
                }
            }

            if (fadeEffect.Enabled)
                Alpha = fadeEffect.Alpha;
        }

        public bool IsMouseClickComps()
        {
            for (int i = 0; i < AllComps.Count; i++)
            {
                if (input.IsMouseClick(AllComps[i].DrawBounds))
                    return true;
            }
            return false;
        }

        public bool IsMouseOverBounds()
        {
            for (int i = 0; i < AllComps.Count; i++)
            {
                if (input.IsMouseOver(AllComps[i].DrawBounds))
                    return true;
            }
            return false;
        }


        public void Unload()
        {
            for (int i = 0; i < AllComps.Count; i++)
                AllComps[i].Unload();

            Game.Components.Remove(this);
        }


    }
}
