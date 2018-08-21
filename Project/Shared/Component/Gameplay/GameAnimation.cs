using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Model;
using Platman.Component.Base;
using Platman.Component.Effect;
using Platman.Component.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platman.Component.Gameplay
{
    public enum GameAnimationPredefined
    {
        Dead,
        NextLevel
    }

    public sealed partial class GameAnimation : DrawableBase
    {
        private const string PATH = "Textures/Gameplay/Animations/";

        public event Handler Started;
        public event Handler Finished;

        public EffectManager EffectManager { get; private set; }

        public GameAnimation(Camera2D camera, Handler method, List<BaseEffect> effects = null) : base(camera)
        {

            effects = effects ?? new List<BaseEffect>();
            List<BaseEffect> list = new List<BaseEffect>();
            list.AddRange(effects);

            _Constructor(list, method);
        }

        public GameAnimation(GameAnimationPredefined anim, Handler method) : base(null)
        {
            Camera = CamManager.Instance.CreateCameraIntance(this);

            switch (anim)
            {
                case GameAnimationPredefined.Dead:
                    _Constructor(LoadDeathAnimation(), method);
                    break;
                case GameAnimationPredefined.NextLevel:
                    _Constructor(LoadNextLevelAnimation(), method);
                    break;
            }
        }

        private void _Constructor(List<BaseEffect> effects, Handler method)
        {

            Started += GameAnimation_Started;
            Finished += GameAnimation_Finished;

            DrawOrder = (int)GameDrawOrder.Person;

            EffectManager = new EffectManager(effects, method);
            EffectManager.Finished += EffectManager_Finished;

            Game.Components.Add(this);
        }

        private void EffectManager_Finished(object sender)
        {
            Finish();
        }

        private void Finish()
        {
            Unload();
            Finished(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            EffectManager.RunEffects(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin(transformMatrix: Camera.Matrix);
            Position += EffectManager.Origin;
            spriteBatch.Draw(EffectManager.Texture, EffectManager.Position, EffectManager.Frame, Color.White * EffectManager.Alpha, EffectManager.Rotation, EffectManager.Origin, 1f, SpriteEffects.None, 0f);
            Position -= EffectManager.Origin;
            spriteBatch.End();
        }

        private void GameAnimation_Finished(object sender)
        {

        }

        private void GameAnimation_Started(object sender)
        {

        }
    }

    // Load Predefined Animations


    public sealed partial class GameAnimation
    {
        private List<BaseEffect> LoadDeathAnimation()
        {
            List<BaseEffect> effects = new List<BaseEffect>();

            TranslationEffect translation = new TranslationEffect(Camera.Viewport.Center.ToVector2(), Camera.Viewport.Center.ToVector2(), new Vector2(50, 0), Vector2.Zero, repeat: true);

            FadeEffect fade = new FadeEffect(false, 200, 1, 0, false, false);
            fade.IsFocus = true;

            AnimationEffect anim2 = new AnimationEffect(new AnimationModel("key1", PATH+"death", 25, 5, 5, 125, false), false);

            effects.Add(translation);

            effects.Add(fade);

            effects.Add(anim2);

            return effects;
        }

        private List<BaseEffect> LoadNextLevelAnimation()
        {
            List<BaseEffect> effects = new List<BaseEffect>();

            FadeEffect fade = new FadeEffect(true, 350, 1, 0, true);

            fade.IsFocus = false;

            DelayEffect delay = new DelayEffect(2000);

            effects.Add(fade);

            effects.Add(delay);

            return effects;
        }
    }
}
