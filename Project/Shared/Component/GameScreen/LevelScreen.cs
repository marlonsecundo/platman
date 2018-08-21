using Model.Screen;
using Platman.Component.Base;
using Platman.Component.GameScreen.Screens.Interface.Button;
using Platman.Component.GameScreen.Screens.Interface.Menu;
using Platman.Component.Managers;
using System.Linq;

namespace Platman.Component.GameScreen.Screens
{
    public partial class LevelScreen : BaseScreen
    {
        private int stageIndex;
        public LevelScreen(int stageIndex) : base(model : LoadLevelScreen(stageIndex))
        {
            this.stageIndex = stageIndex + 1;

            switch (this.stageIndex)
            {
                case 1: Texts[0].Text = "A PROCURA"; break;
                case 2: Texts[0].Text = "Num sei"; break;
            }
        }

        private static ScreenModel LoadLevelScreen(int stageIndex)
        {
            stageIndex++;
            ScreenModel model = ContentLoader.Instance.LoadScreen(ScreenContent.level);
            if (!model.path.Contains("Stage" + stageIndex))
                model.path += (stageIndex) + "/";
            return model;
        }
    }

    public partial class LevelScreen : BaseScreen
    {

        protected override GameButton[] LoadButton()
        {
            return new GameButton[]
          {
                new GameButton(Model.buttons[0], this, (object sender) => { ScreenManager.Instance.SwitchScreen(ScreenContent.main); }, text : Texts.FirstOrDefault(item => item.Text == "VOLTAR"))
          };
        }
        protected override Menu LoadMenu()
        {
            MenuItem[] menuItens = new MenuItem[1];
            menuItens[0] = new MenuItem(0, Input.GameKey.A, StartLevel);
            menuItens[0].AddComp(Arts[0]);

            return new Menu(menuItens, this, ItemOrientation.Horizontal);
        }

        private void StartLevel(object sender)
        {
            int index = (sender as MenuItem).Index + 1;
            ScreenManager.Instance.ShowGameScreen(stageIndex, index);
        }

    }
}
