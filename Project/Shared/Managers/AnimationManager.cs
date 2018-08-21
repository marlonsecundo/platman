using Microsoft.Xna.Framework.Graphics;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace Platman.Component.Managers
{
    public class AnimationManager
    {
        string currentAnimation;

        private Dictionary<string, Animation> listAnimations;

        public Texture2D[] AllTextures
        {
            get
            {
                Texture2D[] texture = new Texture2D[listAnimations.Count];
                string[] keys = listAnimations.Keys.ToArray();

                for (int i = 0; i < texture.Length; i++)
                    texture[i] = listAnimations[keys[i]].Texture;

                return texture;
            }
        }

        public Animation CurrentAnimation
        {
            get { return listAnimations[currentAnimation]; }
        }

        public AnimationManager(AnimationModel[] animationModel)
        {
            Animation[] animations = new Animation[animationModel.Length];
            for (int i = 0; i < animations.Length; i++)
                animations[i] = new Animation(animationModel[i]);

            listAnimations = new Dictionary<string, Animation>();
            for (int i = 0; i < animations.Length; i++)
                listAnimations.Add(animations[i].Key, animations[i]);

            currentAnimation = animations[0].Key;
        }

        public AnimationManager(AnimationModel animationModel)
        {
            Animation animation = new Animation(animationModel);
            listAnimations = new Dictionary<string, Animation>
            {
                { animation.Key, animation }
            };

            currentAnimation = animation.Key;
        }

        public AnimationManager(Animation[] animations)
        {
            listAnimations = new Dictionary<string, Animation>();

            for (int i = 0; i < animations.Length; i++)
                listAnimations.Add(animations[i].Key, animations[i]);

            currentAnimation = animations[0].Key;
        }

        public Animation ChangeAnimation(object animationKey)
        {
            string key = animationKey.ToString();
            currentAnimation = key;
            return listAnimations[key];
        }


    }
}
