
using VelcroPhysics.Collision.Filtering;

namespace Platman.Component.Gameplay.Mechanic.Collision
{
    public class Collide
    {
        // Hero
        public Category HeroWith { get => Category.Cat1; }
        public Category HeroGroup { get => Category.Cat1; }

        // Blocks
        public Category BlockWith { get => Category.Cat1; }
        public Category BlockGroup { get => Category.All; }

        // None
        public Category None { get => Category.None; }

        public static Collide Instance => new Collide() ?? Instance;
        private Collide() { }
    }
}
