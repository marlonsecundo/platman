using Microsoft.Xna.Framework;
using Model.Screen;
using Platman.Component.GameScreen.Screens.Interface;
using Platman.Component.GameScreen.Screens.Interface.Button;
using Platman.Component.GameScreen.Screens.Interface.Menu;
using Platman.Component.Managers;
using System.Collections.Generic;

namespace Platman.Component.Base
{
    public enum ScreenContent
    {
        main,
        about,
        level,
        settings,
        help,
        stage,
        transition,
        input
    }
    public partial class BaseScreen : DrawableBase
    {
        public GameButton[] Buttons { get; private set; }
        public Art[] Arts { get; private set; }
        public GameText[] Texts { get; private set; }
        public DrawableBase[] Comps { get; private set; }
        public List<DrawableBase> AllComps { get; private set; }


        public override int DrawOrder
        {
            get => base.DrawOrder;
            set
            {
                for (int i = 0; i < AllComps.Count; i++)
                    AllComps[i].DrawOrder = value + 1;

                base.DrawOrder = value;
            }
        }

        public override float Alpha
        {
            get => base.Alpha;

            set
            {
                for (int i = 0; i < AllComps.Count; i++)
                    AllComps[i].Alpha = value;

                base.Alpha = value;
            }
        }

        public override Vector2 Position
        {
            get => base.Position;
            set
            {
                Vector2 amount = Position - value;

                for (int i = 0; i < AllComps.Count; i++)
                    AllComps[i].Position += amount;

                base.Position = value;
            }
        }

        public override Camera2D Camera
        {
            get => base.Camera;
            set
            {
                base.Camera = value;

                if (AllComps != null)
                {
                    for (int i = 0; i < AllComps.Count; i++)
                        AllComps[i].Camera = value;
                }
            }
        }

        public override bool Enabled
        {
            get => base.Enabled;

            set
            {
                base.Enabled = value;

                for (int i = 0; i < AllComps.Count; i++)
                    AllComps[i].Enabled = value;
            }
        }

        public override bool Visible
        {
            get => base.Visible;

            set
            {
                base.Visible = value;
                for (int i = 0; i < AllComps.Count; i++)
                    AllComps[i].Visible = value;
            }
        }

        public bool IsLoaded { get; protected set; }
        public ScreenModel Model { get; private set; }
        public BaseScreen(ScreenContent name = ScreenContent.main, ScreenModel model = null) : base(CamManager.Instance.ScreenCamera)
        { 
            Model = model ?? ContentLoader.Instance.LoadScreen(name);

            DrawBounds = Model.bounds;

            for (int i = 0; i < Model.arts.Length; i++)
            {
                for (int j = 0; j < Model.arts[i].animations.Length; j++)
                {
                    if (Model.arts[i].animations[j].texture.Contains(Model.path))
                        break;
                    string s = Model.arts[i].animations[j].texture;
                    Model.arts[i].animations[j].texture = Model.path + s;
                }
            }

            AllComps = new List<DrawableBase>();


            Texts = LoadTexts();
            AllComps.AddRange(Texts);
            
            Arts = LoadArts();
            AllComps.AddRange(Arts);

            Buttons = LoadButton();
            AllComps.AddRange(Buttons);

            Comps = LoadOutherComps();
            AllComps.AddRange(Comps);

            Camera = Camera;

            Menu menu = LoadMenu();
            if (menu != null)
                AllComps.Add(menu);

            Position = Vector2.Zero;

            Enabled = false;
            Visible = false;

            Alpha = 0f;
            DrawOrder = 1;

            Game.Components.Add(this);

        }

        public virtual void Show(float alpha)
        {
            for (float i = 0; i < alpha; i += 0.1f)
                Alpha += i;

            Enabled = true;
            Visible = true;

            IsLoaded = true;
        }

        public virtual void Hide()
        {
            Visible = false;
            Enabled = false;
        }


        protected override void UnloadContent()
        {
            base.UnloadContent();

            for (int i = 0; i < AllComps.Count; i++)
                AllComps[i].Unload();

            Game.Components.Remove(this);
        }

        public override void Unload()
        {
            UnloadContent();
            Game.Components.Remove(this);
        }
    }


    public partial class BaseScreen : DrawableBase
    {
        private Art[] LoadArts()
        {
            var arts = new Art[Model.arts.Length];

            for (int i = 0; i < arts.Length; i++)
                arts[i] = new Art(Model.arts[i]);

            return arts;
        }
        private GameText[] LoadTexts()
        {
            var Texts = new GameText[Model.texts.Length];

            for (int i = 0; i < Texts.Length; i++)
                Texts[i] = new GameText(Model.texts[i]);

            return Texts;
        }

        protected virtual GameButton[] LoadButton()
        {
            return new GameButton[] { };
        }

        protected virtual DrawableBase[] LoadOutherComps()
        {
            return new DrawableBase[] { };
        }

        protected virtual Menu LoadMenu()
        {
            return null;
        }
    }
}
