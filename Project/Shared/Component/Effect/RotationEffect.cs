using Microsoft.Xna.Framework;
using Platman.Component.Base;
using System;

namespace Platman.Component.Effect
{
    public sealed class RotationEffect : BaseEffect
    {
        public static RotationEffect GetRotationUp(int minAngle)
        {
            return new RotationEffect(minAngle, 180, 10, 9f, false, false);
        }

        public static RotationEffect GetRotationDown(int minAngle)
        {
            return new RotationEffect(minAngle, 0, 10, 9f, false, false);
        }

        public static RotationEffect GetRotationLeft(int minAngle)
        {
            return new RotationEffect(minAngle, 90, 10, 9f, false, false);
        }

        public static RotationEffect GetRotationRight(int minAngle)
        {
            return new RotationEffect(minAngle, 270, 10, 9f, false, false);
        }




        private int _sign;

        public int MaxAngle { get; private set; }
        public int MinAngle { get; private set; }
        public float AngleSpeed { get; private set; }
        public int Angle
        {
            get
            {
                return Convert.ToInt32((Radian * 180f / Math.PI));
            }
        }
        public float Radian { get; private set; }
        public float MaxRadian { get; private set; }
        public float MinRadian { get; private set; }
        public float RadianSpeed { get; private set; }
        private int Sign
        {
            get => _sign;
            set
            {
                if (_sign != value)
                    Enabled = true;

                _sign = value;
            }
        }


        public override event EventHandler Finish;

        public RotationEffect(int minAngle, int maxAngle, int delay, float angleSpeed, bool await = false, bool repeat = false) : base(delay, await, repeat)
        {
            Finish += RotationEffect_Finish;
            MaxAngle = maxAngle;
            MinAngle = minAngle;
            AngleSpeed = angleSpeed;

            MaxRadian = (float)(Math.PI * maxAngle / 180f);
            MinRadian = (float)(Math.PI * minAngle / 180f);

            RadianSpeed = (float)(Math.PI * angleSpeed / 180f);
            Sign = maxAngle > minAngle ? 1 : -1;
            Radian = MinRadian;
            Enabled = false;
        }

        private void RotationEffect_Finish(object sender, EventArgs e)
        {

        }

        public override void RunEffect(GameTime gameTime)
        {
            if (Enabled)
            {
                time += gameTime.ElapsedGameTime.Milliseconds;

                if (time >= Delay)
                {
                    time = 0;
                    if (Sign > 0)
                    {
                        Radian += RadianSpeed;

                        if (Radian > MaxRadian)
                        {
                            if (Repeat)
                                Radian = MinRadian;

                            else
                            {
                                Radian = MaxRadian;
                                Enabled = false;
                                Finish(this, null);
                            }
                        }
                    }
                    else
                    {
                        Radian -= RadianSpeed;

                        if (Radian < MaxRadian)
                        {
                            if (Repeat)
                                Radian = MinRadian;

                            else
                            {
                                Radian = MaxRadian;
                                Enabled = false;
                                Finish(this, null);
                            }
                        }
                    }
                }

            }

        }



    }
}
