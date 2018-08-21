using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Model;
using Model.Screen;
using Platman.Component.Base;

using Platman.Component.Input;
using Platman.Component.Managers;
using System;

namespace Platman.Component.GameScreen.Screens.Interface.Button
{
    public enum TimeDelay : int
    {
        Zero = 0,
        asidd = 50,
        Low = 100,
        Middle = 250,
        Big = 400,
        ExtraBig = 600,
        Forever = int.MaxValue
    }

    public class GameButton : AnimatedComponent
    {
        public override float Alpha
        {
            get => base.Alpha;
            set
            {
                base.Alpha = value;
                art.Alpha = value;
                text.Alpha = value;
            }
        }
        public override int DrawOrder
        {
            get => base.DrawOrder;
            set
            {
                base.DrawOrder = value;
                art.DrawOrder = value + 1;
                text.DrawOrder = value + 1;
            }
        }
        public override bool Enabled
        {
            get => base.Enabled;
            set
            {

                base.Enabled = value;
                art.Enabled = value;
                text.Enabled = value;

            }
        }

        public override bool Visible
        {
            get => base.Visible;
            set
            {
                base.Visible = value;
                art.Visible = value;
                text.Visible = value;
            }
        }

        public override Camera2D Camera
        {
            get => base.Camera;
            set
            {
                base.Camera = value;

                if (art != null)
                {
                    art.Camera = value;
                    text.Camera = value;
                }
            }
        }

        public event Handler OnClick;
        public event Handler OnRelease;
        public event Handler OnPressed;

        public GameKey Key { get; set; }
        public ButtonState State { get; set; }
        public TimeDelay Delay { get; set; }

        private GameText text;
        private Art art;


        Vector2 textPos1;
        Vector2 textPos2;

        protected PlayerInput input;
        public GameButton(ButtonModel model, BaseScreen parent, Handler method, Art art= null, GameText text = null) : base(model, LoadButtonAnimationManager(model.name))
        {
            if (parent.AllComps.Contains(art))
                parent.AllComps.Remove(art);
            if (parent.AllComps.Contains(text))
                parent.AllComps.Remove(text);

            this.art = art ?? this.art;
            this.text = text ?? this.text;

            this.text.Foreground = Color.Black;
            textPos1 = this.text.Position;
            textPos2 = textPos1 + new Vector2(0, 5);

            GameKey key;
            Enum.TryParse(model.key, out key);

            OnClick = method;
            Key = key;
            State = ButtonState.Released;
            Delay = (TimeDelay)model.delay;

            OnRelease += Button_OnRelease;
            OnClick += Button_OnClick;
            OnPressed += Button_OnPressed;
        }

        protected override void LoadOutherComps()
        {
            art = DefaultArt;
            text = DefaultText;
        }

        public override void Initialize()
        {
            base.Initialize();
            input = PlayerInput.Instance;
        }


        public override void Update(GameTime gameTime)
        {

            if (State == ButtonState.Pressed)
            {
                time += gameTime.ElapsedGameTime.Milliseconds;
                if (time > (int)Delay)
                {
                    time = 0;
                    OnClick(this);
                    OnRelease(this);
                }
            }

            if (State != ButtonState.Pressed && input.IsMouseClick(DrawBounds) || input.IsKeyDown(Key))
            {
                OnPressed(this);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(transformMatrix: Camera.Matrix);

            spriteBatch.Draw(Texture, Position , Frame, Color.White * Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        private static AnimationManager LoadButtonAnimationManager(string btName)
        {
            Animation[] animations = new Animation[2];

            animations[0] = new Animation(new AnimationModel(ButtonState.Released, "Textures/Input/Released/" + btName, 1, 1, 1, 100, false));
            animations[1] = new Animation(new AnimationModel(ButtonState.Pressed, "Textures/Input/Pressed/" + btName, 1, 1, 1, 100, false));

            AnimationManager anim = new AnimationManager(animations);

            return anim;
        }

        private static Art DefaultArt =>  new Art(new ArtModel(new Vector2(-100, -100), new AnimationModel[] { new AnimationModel("Released", "blank", 1, 1, 1, 100, false), new AnimationModel("Pressed", "blank", 1, 1, 1, 100, false) }));
        private static GameText DefaultText => new GameText(new TextModel(Vector2.Zero, "", "Middle"));

        private void Button_OnRelease(object sender)
        {
            State = ButtonState.Released;
            ChangeAnimation(State);

        }
        private void Button_OnPressed(object sender)
        {
            State = ButtonState.Pressed;
            ChangeAnimation(State);
        }
        private void Button_OnClick(object sender)
        {
            State = ButtonState.Pressed;
            ChangeAnimation(State);
        }

        public override Animation ChangeAnimation(object animationKey)
        {
            switch (State)
            {
                case ButtonState.Pressed:
                    text.Foreground = new Color(172, 50, 50);
                    text.Position = textPos2;
                    break;
                case ButtonState.Released:
                    text.Foreground = Color.Black;
                    text.Position = textPos1;
                    break;
            }

            art.ChangeAnimation(State.ToString());
            return base.ChangeAnimation(animationKey);
        }


        protected void InvokeOnRelease(GameButton b)
        {
            OnRelease(b);
        }
        protected void InvokeOnPressed(GameButton b)
        {
            OnPressed(b);
        }

        protected void InvokeOnClick(GameButton b)
        {
            OnClick(b);
        }

        public override void Unload()
        {
            art.Unload();
            text.Unload();

            base.Unload();
        }
    }
}
