using Platman.Component.Base;
using Platman.Component.GameScreen.Screens.Interface.Button;
using Platman.Component.Managers;
using System.Linq;

namespace Platman.Component.GameScreen.Screens
{
    public partial class HelpScreen : BaseScreen
    {
        public HelpScreen() : base(ScreenContent.help)
        {

        }

        protected override GameButton[] LoadButton()
        {
            return new GameButton[]
            {
                new GameButton(Model.buttons[0], this, (object sender) => { ScreenManager.Instance.SwitchScreen(ScreenContent.main); }, text : Texts.FirstOrDefault(item => item.Text == "VOLTAR"))
            };
        }

     
    }

    public partial class HelpScreen : BaseScreen
    {

    }
}
