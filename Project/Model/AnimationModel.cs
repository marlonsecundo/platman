namespace Model
{
    public class AnimationModel
    {
        public string key;
        public string texture;
        public int frameCount;
        public int rows;
        public int columns;
        public int delay;
        public bool repeat;

        public AnimationModel()
        {

        }

        public AnimationModel(object key, object texture, int frameCount, int rows, int columns, int delay, bool repeat)
        {
            this.key = key.ToString();
            this.texture = texture.ToString();
            this.frameCount = frameCount;
            this.rows = rows;
            this.columns = columns;
            this.delay = delay;
            this.repeat = repeat;
        }
    }
}
