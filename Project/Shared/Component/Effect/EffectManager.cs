using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Model;
using Platman.Component.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Platman.Component.Effect
{
    public class EffectManager
    {
        public event Handler ChangedEffect;
        public event Handler Finished;

        private List<BaseEffect> storyboard;

        // Efeitos que estão executando todos juntos
        private List<BaseEffect> currentEffect;


        public float Alpha { get; private set; }
        public Vector2 Position { get; private set; }
        public float Rotation { get; private set; }
        public Texture2D Texture { get => Animation.Texture; }
        public Rectangle Frame { get => Animation.Frame; }
        public Vector2 Origin { get => Animation.CenterPosition; }
        private Animation Animation { get; set; }

        // Efeitos que estão executando separados
        private FadeEffect currentFadeEffect;
        private TranslationEffect currentTranslationEffect;
        private RotationEffect currentRotationEffect;
        private AnimationEffect currentAnimationEffect;

        public Handler UpdateMethod;

        public EffectManager(List<BaseEffect> effects, Handler method)
        {
            UpdateMethod = method;

            Finished += EffectManager_Finished;
            ChangedEffect += EffectManager_ChangedEffect;

            storyboard = new List<BaseEffect>();
            currentEffect = new List<BaseEffect>();

            storyboard = effects;

            for (int i = 0; i < storyboard.Count; i++)
                storyboard[i].Finish += EffectManager_Finish;

            for (int i = 0; i < storyboard.Count; i++)
            {
                currentEffect.Add(storyboard[i]);

                ChangeCurrentEffect(storyboard[i]);

                if (storyboard[i].Await)
                    break;
            }

            Alpha = currentFadeEffect != null ? currentFadeEffect.Alpha : 1f;
            Rotation = currentRotationEffect != null ? currentRotationEffect.Radian : Rotation;
            Position = currentTranslationEffect != null ? currentTranslationEffect.Position : Position;
            Animation = currentAnimationEffect != null ? currentAnimationEffect.Animation : new Animation(new AnimationModel("key1", "Textures/blank", 1,1,1, int.MaxValue, false));

        }


        private void ChangeCurrentEffect(BaseEffect effect)
        {
            Type type = effect.GetType();

            if (type == typeof(FadeEffect))
                currentFadeEffect = (FadeEffect)effect;
            else if (type == typeof(TranslationEffect))
                currentTranslationEffect = (TranslationEffect)effect;
            else if (type == typeof(RotationEffect))
                currentRotationEffect = (RotationEffect)effect;
            else if (type == typeof(AnimationEffect))
                currentAnimationEffect = (AnimationEffect)effect;
        }

        private void RemoveCurrentEffect(BaseEffect effect)
        {
            Type type = effect.GetType();

            if (type == typeof(FadeEffect))
                currentFadeEffect = null;
            else if (type == typeof(TranslationEffect))
                currentTranslationEffect = null;
            else if (type == typeof(RotationEffect))
                currentRotationEffect = null;
        }

        private void EffectManager_Finish(object sender, EventArgs e)
        {
            // Antes de retirar o efeito atualizar a ultima infromação que ele guarda 
            Alpha = currentFadeEffect != null ? currentFadeEffect.Alpha : Alpha;
            Rotation = currentRotationEffect != null ? currentRotationEffect.Radian : Rotation;
            Position = currentTranslationEffect != null ? currentTranslationEffect.Position : Position;
            Animation = currentAnimationEffect != null ? currentAnimationEffect.Animation : Animation;

            UpdateMethod(this);


            int index = storyboard.IndexOf((sender as BaseEffect));

            // Ja que terminou remove dos efeitos separados
            RemoveCurrentEffect(storyboard[index]);

            currentEffect.Remove(storyboard[index]);

            storyboard.RemoveAt(index);

            // Se o ultimo efeito não for de esperar
            if (currentEffect.Count > 0 && !currentEffect.Last().Await)
            {
                // Começa pelo index que ja removeu, pra adicionar o proximo efeito
                for (int i = index; i < storyboard.Count; i++)
                {
                    if (!currentEffect.Contains(storyboard[i]))
                    {
                        var effect = storyboard[i];
                        bool exist = false;

                        // Verificar se existe o efeito do mesmo tipo rodando já
                        for (int j = 0; j < currentEffect.Count; j++)
                        {
                            if (currentEffect[j].GetType() == effect.GetType())
                            {
                                exist = true;

                                currentEffect[j] = effect;

                                ChangeCurrentEffect(effect);
                            }
                        }

                        // Se não existe efeito do mesmo tipo rodando.... ADICIONAR
                        if (!exist)
                        {
                            currentEffect.Add(storyboard[index]);
                            ChangeCurrentEffect(storyboard[index]);
                        }
                        // Se for um efeito de esperar pra acabar.... parar de adicionar no current
                        if (storyboard[i].Await)
                            break;
                    }
                }
            }
        }

        public void RunEffects(GameTime gameTime)
        {
            Alpha = currentFadeEffect != null ? currentFadeEffect.Alpha : Alpha;
            Rotation = currentRotationEffect != null ? currentRotationEffect.Radian : Rotation;
            Position = currentTranslationEffect != null ? currentTranslationEffect.Position : Position;
            Animation = currentAnimationEffect != null ? currentAnimationEffect.Animation : Animation;

            var effects = currentEffect.ToArray();
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].RunEffect(gameTime);
            }

            if (currentEffect.Count == 0 || currentEffect.All(t => t.Repeat == true))
            {
                // Só está executando aqueles que são infinitos
                Finish();
                return;
            }

            UpdateMethod(this);

        }

        private void Finish()
        {
            Finished(this);
            storyboard.RemoveAll(t => t != null);
        }


        private void EffectManager_ChangedEffect(object sender)
        {

        }

        private void EffectManager_Finished(object sender)
        {

        }
    }
}
