using Platman;
using System;

namespace Desktop
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameRoot())
            {
                Device.Init(DType.Desktop, new DesktopData(game));
                game.Run();
            }


        }
    }
}
