using Microsoft.Xna.Framework;

namespace Platman.Extensions
{
    public static class MatrixExt
    {
        public static Vector2 Multiply(this Matrix value1, Vector2 value2)
        {
            return new Vector2(
                ((value1.M11 * value2.X) + (value1.M21 * value2.Y) + value1.M41),
                ((value1.M12 * value2.X) + (value1.M22 * value2.Y) + value1.M42));
        }
    }
}
