using Model.Gameplay;
using VelcroPhysics.Dynamics;

namespace Platman.Component.Gameplay.Map
{
    public class TextPoint : Block
    {
        public string Text { get; }
        public TextPoint(TextPointModel model, World world) : base(model, world)
        {
            Text = model.text;
        }
    }
}
