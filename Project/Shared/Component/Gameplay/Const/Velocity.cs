using Microsoft.Xna.Framework;
using Platman.Component.Gameplay.Mechanic.Gravity;

namespace Platman.Component.Gameplay.Const
{
    public class VelocityValues
    {
        private static VelocityValues _instance;
        public static VelocityValues Instance => _instance;

        public static void Init(GravityManager gravityManager)
        {
            if (_instance == null)
                _instance = new VelocityValues(gravityManager);
        }

        private VelocityValues(GravityManager gravityManager)
        {
            GravityManager = gravityManager;
        }

        public GravityManager GravityManager { get; }
        public Vector2 Down => new Vector2(0, 13f);
        public Vector2 Move => new Vector2(6f, 0);
        public Vector2 Run => new Vector2(6f, 0);
        public Vector2 Stop =>new Vector2(7f, 0);
        public Vector2 JumpImpulse => new Vector2(0, -7f);
        public Vector2 RunImpulse => new Vector2(7f, 0);
        public Vector2 GroundImpulse => new Vector2(3f, 0);

        public Vector2 GravityDown => new Vector2(0, 10f);
        public Vector2 GravityLeft => new Vector2(-10f, 0);
        public Vector2 GravityRight => new Vector2(10f, 0);
        public Vector2 GravityUp => new Vector2(0, -10f);

    }
}
