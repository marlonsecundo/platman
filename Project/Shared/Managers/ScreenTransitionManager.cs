using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platman.Component.Base;
using Platman.Component.Effect;
using Platman.Component.GameScreen.Screens;
using Platman.Component.Input;
using System;

namespace Platman.Component.Managers
{
    public delegate void TransitionEventHandler();
    public delegate void ShowScreenHandler(BaseScreen screen, float alpha);

    public partial class ScreenTransitionManager : BaseScreen
    {
        public event TransitionEventHandler Finished;
        private ShowScreenHandler ShowScreenMethod;

        private BaseScreen screen;
        private BaseScreen previousScreen;
        private FadeEffect fadeEffect;

        private int timeSleep;
        private bool canTransit;

        private Texture2D background;

        protected SpriteBatch spritebatch = new SpriteBatch(GameRoot.Instance.GraphicsDevice);

        private ScreenTransitionManager() : base(ScreenContent.transition)
        {
            Enabled = false;
            Visible = false;
            Finished += FinishTransition;
            background = ContentLoader.Instance.LoadTextureByPath("Textures/Screen/Transition/back");
            Alpha = 0f;
        }

        private void TransitionScreenManager_Loading()
        {

        }

        public void StartTransition(BaseScreen screen, BaseScreen previousScreen, ShowScreenHandler showScreenMethod)
        {
            PlayerInput.Instance.Enabled = false;

            ShowScreenMethod = showScreenMethod;

            fadeEffect = new FadeEffect(false, 35, 1f, 0f);
            fadeEffect.Finish += FadeEffect_Completed;
            fadeEffect.IsFocus = true;

            this.screen = screen;
            this.previousScreen = previousScreen;

            base.Show(0f);

            Enabled = true;
            Visible = true;

            DrawOrder = 2;

        }


        private void FadeEffect_Completed(object sender, EventArgs e)
        {
            if (!fadeEffect.IsFocus)
            {

            }
            else
            {
                Audio.GameMusicPlayer.Instance.StopMusic();
                BackgroundScreen.Instance.Unload();
                previousScreen.Enabled = false;
                ShowScreenMethod(screen, 0f);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float alpha = fadeEffect.Alpha;

            fadeEffect.RunEffect(gameTime);

            if (alpha != fadeEffect.Alpha && fadeEffect.Enabled)
            {
                Alpha = fadeEffect.Alpha;
                // Ir diminuindo o som
                if (!canTransit)
                    Microsoft.Xna.Framework.Media.MediaPlayer.Volume -= 0.1f;
            }

            if (fadeEffect.Alpha >= 1f && screen.IsLoaded)
            {
                // Tempo de Espera
                timeSleep += gameTime.ElapsedGameTime.Milliseconds;

                if (!canTransit && timeSleep > 1500)
                {
                    canTransit = true;
                    fadeEffect.IsFocus = false;
                }
            }

            if (canTransit)
                // Se pode transitar, aumenta o alpha da nova tela 
                screen.Alpha = 1f - fadeEffect.Alpha;
            else
            {
                // Diminuindo o alpha da tela anterior (somente diminuir se o valor for menor do que o alpha dele)
                float value = 1f - fadeEffect.Alpha;
                if (previousScreen.Alpha >= value)
                    previousScreen.Alpha = value;
            }

            if (!fadeEffect.IsFocus && !fadeEffect.Enabled)
            {
                Enabled = false;
                Finished();
            }

        }

        public void FinishTransition()
        {
            Unload();
            PlayerInput.Instance.Enabled = true;
        }

    }


    public partial class ScreenTransitionManager : BaseScreen
    {
        public override void Show(float alpha)
        {
            throw new Exception("Você tentou acessar o metodo show de fora");
        }

        public override void Draw(GameTime gameTime)
        {
            spritebatch.Begin();

            spritebatch.Draw(background, Vector2.Zero, DrawBounds, Color.White * Alpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);

            spritebatch.End();

            base.Draw(gameTime);
        }

    }


    public partial class ScreenTransitionManager : BaseScreen
    {
        private static ScreenTransitionManager _instance;

        public static ScreenTransitionManager NewInstance
        {
            get
            {
                if (_instance == null)
                    _instance = new ScreenTransitionManager();
                else
                {
                    _instance.Unload();
                    _instance = new ScreenTransitionManager();
                }

                return _instance;
            }
        }
    }
}
