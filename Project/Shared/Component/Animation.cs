using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Model;

namespace Platman.Component
{
    public sealed class Animation
    {
        public Vector2 CenterPosition { get; }
        public Rectangle Frame { get { return new Rectangle(Position.ToPoint() * Bounds.Size, Bounds.Size); } }
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        public Rectangle Bounds { get; }
        public string Key { get; }
        public bool Repeat { get; }
        public int FrameCount { get; }
        public int FrameIndex { get => System.Convert.ToInt32((Position.X * Position.Y) + Position.X); }
        public int Rows { get; }
        public int Columns { get; }
        public int Delay { get; set; }
        public int TotalTime { get; }
        public bool Completed { get => completedNext || completedPrevious; }

        private bool completedNext = false;
        private bool completedPrevious = false;
        private int time;
        public Animation(AnimationModel model)
        {
            Key = model.key;
            Position = Vector2.Zero;
            FrameCount = model.frameCount;
            Delay = model.delay;
            TotalTime = Delay * model.frameCount;
            Repeat = model.repeat;
            Columns = model.columns;
            Rows = model.rows;
            

            Texture = ContentLoader.Instance.LoadTextureByPath(model.texture);

            Bounds = new Rectangle(0, 0, Texture.Width / Columns, Texture.Height / Rows);

            CenterPosition = new Vector2(Bounds.Width / 2f, Bounds.Height / 2f);
        }

        public void NextFrame(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.Milliseconds;

            if (time >= Delay)
            {
                time = 0;

                switch (Repeat)
                {
                    case true: NextFrameRepeat(); break;
                    case false: NextFrameAtLimit(); break;
                }
            }
        }

        public void JumpToFrame(int frame)
        {
            frame--;

            int row = 0;
            int column = 0;
            for (int i = 0; i < frame; i++)
            {
                column++;

                if (column > Columns - 1)
                {
                    column = 0;
                    row++;
                }
            }
            
            Vector2 pos = new Vector2(column, row);

            // Resete do Frame
            if (Columns * pos.Y + pos.X < FrameCount)
            {
                Position = pos;
            }
        }

        private bool IsInside(Vector2 position)
        {
            Rectangle bounds = new Rectangle(position.ToPoint() * Bounds.Size, Bounds.Size);

            float length = position.Y * Columns + position.X + 1;
            
            if (length <= FrameCount && Texture.Bounds.Contains(bounds))
                return true;

            return false;
        }

        private void NextFrameRepeat()
        {
            Position += new Vector2(1, 0);

            // Se a nova posição não estiver dentro da textura
            if (!IsInside(Position))
            {
                // Desce uma linha
                Position = new Vector2(0, Position.Y + 1);

                // Se ainda não estiver dentro da textura volta pra posição inicial
                if (!IsInside(Position))
                    Position = Vector2.Zero;
                
            }
        }
        
        private void PreviousFrameRepeat()
        {
            Position += new Vector2(-1, 0);

            // Se a nova posição não estiver dentro da textura
            if (!IsInside(Position))
            {
                // Sobe uma linha
                Position = new Vector2(0, Position.Y - 1);

                // Se ainda não estiver dentro da textura volta pra posição inicial
                if (!IsInside(Position))
                    Position = Vector2.Zero;

            }
        }

        private void NextFrameAtLimit()
        {
            if (!completedNext)
            {
                Vector2 pos = Position;
                pos += new Vector2(1, 0);

                // Se a nova posição não estiver dentro da textura
                if (!IsInside(pos))
                {
                    // Desce uma linha
                    pos = new Vector2(0, pos.Y + 1);

                    // Se ainda não estiver dentro da textura volta pra posição inicial
                    if (!IsInside(pos))
                        completedNext = true;

                }

                if (!completedNext)                
                    Position = pos;
                

                completedPrevious = false;
            }
        }

        private void PreviousFrameAtLimit()
        {
            if (!completedPrevious)
            {
                Vector2 pos = Position;
                pos += new Vector2(-1, 0);

                // Se a nova posição não estiver dentro da textura
                if (!IsInside(pos))
                {
                    // Sobe uma linha
                    pos = new Vector2(0, pos.Y - 1);

                    // Se ainda não estiver dentro da textura volta pra posição inicial
                    if (!IsInside(pos))
                        completedPrevious = true;

                }


                if (!completedPrevious)
                    Position = pos;

                completedNext = false;
            }
        }

        public void ResetAnimation()
        {
            Position = Vector2.Zero;

            if (!Repeat)
            {
                completedPrevious = false;
                completedNext = false;
            }
        }

        private void Animation_Finished()
        {

        }

    }
}
