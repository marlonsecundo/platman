using Microsoft.Xna.Framework;
using Platman.Component.Base;
using Platman.Component.GameScreen.Screens.Interface.Menu;
using Platman.Component.Input;
using Platman.Component.Managers;
using System;
using System.Linq;

namespace Platman.Component.GameScreen.Screens
{
    public class MainScreen : BaseScreen
    {
        Menu menu;
        public MainScreen() : base(ScreenContent.main)
        {
            Random rnd = new Random();
            int index = rnd.Next(1, 3);
            Arts[0].ChangeAnimation("key" + index);
            menu = (Menu)AllComps.First(t => t is Menu);
        }

        public override void Show(float alpha)
        {
            base.Show(alpha);
            var init = BackgroundScreen.Instance;

            menu.FocusItem(0, false);
        }

        public override void Hide()
        {
            base.Hide();
            menu.DeFocus();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }



        protected override Menu LoadMenu()
        {
            MenuItem[] menuItens = new MenuItem[5];

            menuItens[0] = new MenuItem(0, GameKey.A, (object sender) => {  ScreenManager.Instance.SwitchScreen( ScreenContent.stage); });
            menuItens[1] = new MenuItem(1, GameKey.A, (object sender) => { ScreenManager.Instance.SwitchScreen(ScreenContent.settings); });
            menuItens[2] = new MenuItem(2, GameKey.A, (object sender) => { ScreenManager.Instance.SwitchScreen(ScreenContent.help); });
            menuItens[3] = new MenuItem(3, GameKey.A, (object sender) => { ScreenManager.Instance.SwitchScreen(ScreenContent.about); });
            menuItens[4] = new MenuItem(4, GameKey.A, (object sender) => { Device.Instance.PlatData.Exit(); });

           
            menuItens[0].AddComp(Texts[0]);
            menuItens[1].AddComp(Texts[1]);
            menuItens[2].AddComp(Texts[2]);
            menuItens[3].AddComp(Texts[3]);
            menuItens[4].AddComp(Texts[4]);

            return new Menu(menuItens, this, ItemOrientation.Vertical);
        }
    }
}
