namespace Platman.Debug
{
    public class DebugText
    {
        public string Text { get; }
        public int Timeout { get; }

        public int time;

        public bool visible;

        public DebugText(string text, int timeout)
        {
            Text = text;
            Timeout = timeout;
        }
    }
}
