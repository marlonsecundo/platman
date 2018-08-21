using Platman;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Win10
{
    public sealed partial class GamePage : Page
    {
		readonly GameRoot _game;

		public GamePage()
        {
            this.InitializeComponent();

            Device.Init(DType.Win10_PC, new Win10Data());

			var launchArguments = string.Empty;
            _game = MonoGame.Framework.XamlGame<GameRoot>.Create(launchArguments, Window.Current.CoreWindow, swapChainPanel);

        }
    }
}
