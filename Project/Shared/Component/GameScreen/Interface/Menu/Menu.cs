using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Platman.Component.Audio;
using Platman.Component.Base;
using Platman.Component.Input;
using Platman.Component.Managers;

namespace Platman.Component.GameScreen.Screens.Interface.Menu
{
    public enum ItemOrientation
    {
        Vertical,
        Horizontal,
    }
    public sealed class Menu : DrawableBase
    {
        public int ItemIndex { get; private set; }

        public override float Alpha
        {
            get => base.Alpha;
            set
            {
                base.Alpha = value;
                for (int i = 0; i < Itens.Length; i++)
                    Itens[i].Alpha = value;
            }
        }

        public override bool Enabled
        {
            get => base.Enabled;
            set
            {
                base.Enabled = value;
                for (int i = 0; i < Itens.Length; i++)
                    Itens[i].Enabled = value;
            }
        }

        public override bool Visible
        {
            get => base.Visible;
            set
            {
                base.Visible = value;
                for (int i = 0; i < Itens.Length; i++)
                    Itens[i].Visible = value;
            }
        }

        public override Vector2 Position
        {
            get => base.Position;
            set
            {
                base.Position = value;

                for (int i = 0; i < Itens.Length; i++)
                    Itens[i].Position = value;
            }
        }

        public override int DrawOrder
        {
            get => base.DrawOrder;
            set
            {
                base.DrawOrder = value;

                for (int i = 0; i < Itens.Length; i++)
                    Itens[i].DrawOrder = value;
            }
        }

        public MenuItem[] Itens { get; private set; }

        private int time;
        private bool indexChange = false;
        private SoundEffect soundEffect;
        private PlayerInput input;

        private GameKey up;
        private GameKey down;
        public Menu(MenuItem[] menuItens, BaseScreen parent, ItemOrientation orientation) : base(CamManager.Instance.ScreenCamera)
        {

            Itens = menuItens;

            foreach (MenuItem item in menuItens)
            {
                item.OnFocus += FocusItem;
            }
            // Retirar o controle do componente do Screen para o MenuItem
            for (int i = 0; i < menuItens.Length; i++)
            {
                for (int j = 0; j < menuItens[i].AllComps.Count; j++)
                {
                    parent.AllComps.Remove(menuItens[i].AllComps[j]);
                }
            }

            switch (orientation)
            {
                case ItemOrientation.Horizontal:
                    up = GameKey.Left;
                    down = GameKey.Right;
                    break;
                case ItemOrientation.Vertical:
                    up = GameKey.Up;
                    down = GameKey.Down;
                    break;

            }

            soundEffect = ContentLoader.Instance.LoadSound("screen");

            Camera = parent.Camera;

            Game.Components.Add(this);

        }

        public override void Initialize()
        {
            base.Initialize();
            input = PlayerInput.Instance;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (indexChange)
            {
                time += gameTime.ElapsedGameTime.Milliseconds;

                if (time > 100)
                {
                    time = 0;
                    indexChange = false;
                }
            }
            else
            {

                if (input.IsKeyDown(down))
                {
                    FocusItem(++ItemIndex);
                }
                else if (input.IsKeyDown(up))
                {
                    FocusItem(--ItemIndex);
                }
            }
        }

        public override void Unload()
        {
            for (int i = 0; i < Itens.Length; i++)
                Itens[i].Unload();

            Game.Components.Remove(this);
        }

        public void DeFocus()
        {
            for (int i = 0; i < Itens.Length; i++)
                Itens[i].IsFocused = false;            
        }


        public void FocusItem(int index, bool sound = true)
        {
            int previous = ItemIndex;

            ItemIndex = index;

            if (ItemIndex >= Itens.Length)
                ItemIndex = Itens.Length - 1;
            else if (ItemIndex < 0)
                ItemIndex = 0;

            if (!Itens[ItemIndex].Enabled)
            {
                ItemIndex = previous;
                return;
            }

            if (!Itens[ItemIndex].IsFocused)
            {
                indexChange = true;

                for (int i = 0; i < Itens.Length; i++)
                    Itens[i].IsFocused = false;
                
                if (sound) GameSoundPlayer.Instance.PlaySound(soundEffect);

                Itens[ItemIndex].IsFocused = true;
                ItemIndex = ItemIndex;
            }
        }
    }
}
