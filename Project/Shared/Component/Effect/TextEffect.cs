using Microsoft.Xna.Framework;
using Platman.Component.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platman.Component.Effect
{
    public class TextEffect : BaseEffect
    {
        public override event EventHandler Finish;

        public string Text { get; private set; }
        public string[] Verses { get; private set; }

        public TextEffect(string text, int delay, bool await = false, bool repeat = false) : base(delay, await, repeat)
        {
            Finish += TextEffect_Finish;

            var array = text.Split('[');

            // Contar a quantidade de parágrafo e depois colocar na lista string

            int count = 0;

            for (int i = 0; i < text.Length; i++)
            {
                if (text.Substring(i, 1) == "[")
                    count++;
            }

            List<string> list = new List<string>();

            for (int i = 0; i < array.Length; i++)
            {
                list.Add(array[i]);

                if (i < count)
                    list.Add(new StringBuilder().AppendLine().ToString());
            }

            Verses = list.ToArray();
        }

        private void TextEffect_Finish(object sender, EventArgs e)
        {

        }

        public override void RunEffect(GameTime gameTime)
        {
            if (Enabled)
            {

            }
        }
    }
}
