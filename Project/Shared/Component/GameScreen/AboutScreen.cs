using Microsoft.Xna.Framework;
using Platman.Component.Base;
using Platman.Component.GameScreen.Screens.Interface.Button;
using Platman.Component.Managers;
using System.Linq;
namespace Platman.Component.GameScreen.Screens
{
    public class AboutScreen : BaseScreen
    {
        public AboutScreen() : base(ScreenContent.about)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override GameButton[] LoadButton()
        {
            return new GameButton[] 
            {
                new GameButton(Model.buttons[0], this, (object sender) => { ScreenManager.Instance.SwitchScreen(ScreenContent.main); }, text : Texts.FirstOrDefault(item => item.Text == "VOLTAR"))
            };
        }

    }
}
