using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Platman;

namespace Android
{
    [Activity(Label = "Android"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Landscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            Device.Init(DType.Android, new AndroidData(this));            

            var g = new GameRoot();
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }
    }
}

