
using Microsoft.Xna.Framework;

namespace Platman.Extensions
{
    public enum Angle
    {
        Horizontal
    }
    public struct RectangleFloat
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public float Top { get; private set; } 
        public float Bottom { get; private set; }
        public float Left { get; private set; }
        public float Right { get; private set; }

        public RectangleFloat(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;

            Top = Position.Y - Size.Y / 2f;
            Bottom = Position.Y + Size.Y / 2f;
            Left = Position.X - Size.X / 2f;
            Right = Position.X + Size.X / 2f;
        }

        public RectangleFloat TransformRectangle(Angle angle)
        {
            Vector2 left = new Vector2(Left, Top);
            left = Vector2.Transform(left, Matrix.CreateRotationY(MathHelper.ToRadians(-90)));

            switch(angle)
            {
                case Angle.Horizontal:
                    Top = Position.Y - Size.X / 2;
                    Bottom = Position.Y + Size.X / 2;
                    Left = Position.X - Size.Y / 2;
                    Right = Position.X + Size.Y / 2;
                    break;
            }

            return this;
        }

        public bool Contains(RectangleFloat value)
        {
            if (Left <= value.Left && Right >= value.Right && Top <= value.Top && Bottom >= value.Bottom)
                return true;

            return false;
        }
    }
}
