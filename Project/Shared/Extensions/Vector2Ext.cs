using Microsoft.Xna.Framework;
using System;

namespace Platman.Extensions
{
    public static class Vector2Ext
    {
        public static Vector2 Abs(this Vector2 value1)
        {
            return new Vector2(Math.Abs(value1.X), Math.Abs(value1.Y));
        }
    }
}
