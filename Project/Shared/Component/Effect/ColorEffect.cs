using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Platman.Component.Effect
{
    public class ColorEffect : Base.BaseEffect
    {
        public Color[] OldColors { private set; get; }
        public Color[] NewColors { private set; get; }
        public Texture2D[] Textures { private set; get; }
        public ColorEffect(Color[] oldColors, Color[] newColors, Texture2D[] textures) : base(0, false, false)
        {
            OldColors = oldColors;
            NewColors = newColors;
            Textures = textures;
        }

        public override event EventHandler Finish;

        public override void RunEffect(GameTime gameTime)
        {
            for (int t = 0; t < Textures.Length; t++)
            {
                Texture2D texture = Textures[t];

                Color[] colors = new Color[texture.Width * texture.Height];

                texture.GetData(colors);


                for (int i = 0; i < colors.Length; i++)
                {
                    for (int j = 0; j < OldColors.Length; j++)
                    {
                        if (colors[i] == OldColors[j])
                        {
                            colors[i] = NewColors[j];
                            break;
                        }
                    }
                }

                texture.SetData(colors);
            }
        }
    }
}
