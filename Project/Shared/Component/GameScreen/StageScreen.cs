using Platman.Component.Base;
using Platman.Component.GameScreen.Screens.Interface.Button;
using Platman.Component.GameScreen.Screens.Interface.Menu;
using Platman.Component.Input;
using Platman.Component.Managers;
using System.Linq;

namespace Platman.Component.GameScreen.Screens
{
    public sealed class StageScreen : BaseScreen
    {
        private Menu menu;
        public StageScreen() : base(ScreenContent.stage)
        {

        }

        protected override GameButton[] LoadButton()
        {
            return new GameButton[]
            {
                new GameButton(Model.buttons[0], this, (object sender) => { ScreenManager.Instance.SwitchScreen(ScreenContent.main); }, text :  Texts.FirstOrDefault(item => item.Text == "VOLTAR"))
            };
        }

        protected override Menu LoadMenu()
        {
            MenuItem[] menuItens = new MenuItem[1];

            for (int i = 0; i < menuItens.Length; i++)
                menuItens[i] = new MenuItem(i, GameKey.A, ButtonStage_OnClick);

            menuItens[0].AddComp(Arts[0]);
            menuItens[0].AddComp(Texts[1]);
            

            menu = new Menu(menuItens, this, ItemOrientation.Horizontal);
            return menu;
        }

        public void ButtonStage_OnClick(object sender)
        {
            ScreenManager.Instance.SwitchLevelScreen(menu.ItemIndex);
        }


    }
}
