using Foundation;
using Platman;
using UIKit;
namespace IOS
{
    [Register("AppDelegate")]
    class Program : UIApplicationDelegate
    {
        private static GameRoot game;

        internal static void RunGame()
        {
            Device.Init(DType.IOS, new IOSAdapter());
            game = new GameRoot();
            game.Run();    
        }
        
        static void Main(string[] args)
        {
            UIApplication.Main(args, null, "AppDelegate"); 
        }

        public override void FinishedLaunching(UIApplication app)
        {
            RunGame();
        }
    }
}
