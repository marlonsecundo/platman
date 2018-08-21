using Microsoft.Xna.Framework;
using Model.Base;
using Model.Screen;
using Platman.Component.Base;
using Platman.Component.Gameplay.Map;
using Platman.Component.GameScreen;
using Platman.Component.GameScreen.Screens;
using Platman.Component.Input;
using Platman.Component.Managers;

namespace Platman.Component.Gameplay
{
    public sealed partial class GameLevelScreen : BaseScreen
    {
        private Level Level { get; set; }
        public bool IsLevelLoaded { get; set; }

        public override float Alpha
        {
            get => base.Alpha;
            set => base.Alpha = value;
        }

        public GameLevelScreen(int stage, int level) : base(model : new ScreenModel("", new ArtModel[0], new ButtonModel[0], new TextModel[0], new CompModel[0], CamManager.Instance.LevelCamera.Viewport))
        {
            LoadLevel(stage, level);
            Level.Camera = CamManager.Instance.LevelCamera;

            AllComps.Add(Level);

            Enabled = false;
            Visible = false;

            Alpha = 0f;
            DrawOrder = 1;
        }

        public void StartLevel()
        {
            Enabled = true;
            Level.Start();
        }

        public void ResetLevel()
        {
            int stage = Level.StageIndex;
            int level = Level.LevelIndex;
            Unload();
            ScreenManager.Instance.ShowGameScreen(stage, level);
        }

        public void NextLevel()
        {
            int stage = Level.StageIndex + 1;
            int level = Level.LevelIndex + 1;
            Unload();
            ScreenManager.Instance.ShowGameScreen(stage, level);
        }

        public void LoadLevel(int stage, int level)
        {
            Level = new Level(stage, level, ContentLoader.Instance.LoadLevel(stage, level), this);
            IsLevelLoaded = true;
        }

    }





    public sealed partial class GameLevelScreen
    {

        public override void Show(float alpha)
        {
            base.Show(alpha);
            ScreenManager.Instance.TransitionComplete += ScreenManager_TransitionComplete;
            PlayerInput.Instance.Enabled = false;
            Enabled = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (PlayerInput.Instance.IsKeyDown(GameKey.Esc))
                ScreenManager.Instance.SwitchScreen(new MainScreen());
        }
        private void ScreenManager_TransitionComplete()
        {
            StartLevel();
        }

        public override void Unload()
        {
            base.Unload();
            ScreenManager.Instance.TransitionComplete -= ScreenManager_TransitionComplete;
        }

    }
}
