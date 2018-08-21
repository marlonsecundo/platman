using Microsoft.Xna.Framework;
using Model.Gameplay;
using Platman.Component.Base;
using Platman.Component.Effect;
using Platman.Component.Gameplay.Const;
using Platman.Component.Gameplay.Mechanic;
using Platman.Component.Gameplay.Mechanic.Gravity;
using Platman.Component.Gameplay.Person;
using Platman.Component.Input;
using Platman.Component.Managers;
using System;
using System.Collections.Generic;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Utilities;

namespace Platman.Component.Gameplay.Map
{

    public enum LevelUpdateOrder : int
    {
        Person = 2,
        Level = 1,
        Blocks = 0,

    }

    public delegate Vector2 ColideHandler(Vector2 position);
    public sealed partial class Level : DrawableBase
    {
        public int LevelIndex { get; }
        public int StageIndex { get; }
        public override float Alpha
        {
            get => base.Alpha;

            set
            {
                base.Alpha = value;

                for (int i = 0; i < AllComps.Count; i++)
                    AllComps[i].Alpha = value;
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

                GravityManager.Enabled = value;
            }
        }
        public GameLevelScreen ScreenParent { get; }
        public List<Block> Blocks { get; private set; }
        public List<DeathBlock> DeathBlocks { get; private set; }
        public List<TextPoint> TextPoints { get; private set; }
        public List<MusicPoint> MusicPoints { get; private set; }
        public List<PoetryPoint> PoetryPoints { get; private set; }
        public List<GravityPoint> GravityPoints { get; private set; }
        public List<DrawableBase> AllComps { get; private set; }
        public List<Block> FinalPoints { get; private set; }
        public Hero Platman { get; set; }
        private Background Background { get; set; }
        public  GravityManager GravityManager { get; private set; }

        private Vector2 cameraStartPos;

        public World World { get; private set; }

        public Level(int stage, int level, LevelModel model, GameLevelScreen screen) : base(CamManager.Instance.LevelCamera)
        {
            StageIndex = stage;
            LevelIndex = level;

            _LoadContent(model);
            _Constructor();

            ScreenParent = screen;
            Game.Components.Add(this);
        }

        public void Start()
        {
            Enabled = false;

            for (int i = 0; i < Blocks.Count; i++)
                Blocks[i].Enabled = false;

            Platman.Visible = true;

            Platman.Alpha = 0;

            PlayerInput.Instance.Enabled = false;

            new GameAnimation(CamManager.Instance.LevelCamera, UpdateShowPlatmanAnimation, LoadShowPlatmanAnimation()).Finished += Platman_Showed;
        }

        private void _LoadContent(LevelModel model)
        {
            {
                World = new World(Vector2.Zero);
                ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
            }

            cameraStartPos = model.camera.position;

            DrawBounds = model.bounds;

            Camera.Position = cameraStartPos;
            Camera.ViewArea = model.bounds;

            GravityManager = new GravityManager(World);

            VelocityValues.Init(GravityManager);

            World.Gravity = VelocityValues.Instance.GravityDown;

            Background = new Background(model.background);

            Platman = new Hero(model.hero, World, GravityManager);

            Blocks = new List<Block>();
            for (int i = 0; i < model.blocks.Count; i++)
                Blocks.Add(new Block(model.blocks[i], World));

            DeathBlocks = new List<DeathBlock>();
            for (int i = 0; i < model.deathPoints.Count; i++)
                DeathBlocks.Add(new DeathBlock(model.deathPoints[i], World));

            TextPoints = new List<TextPoint>();
            for (int i = 0; i < model.textPoints.Count; i++)
                TextPoints.Add(new TextPoint(model.textPoints[i], World));

            MusicPoints = new List<MusicPoint>();
            for (int i = 0; i < model.musicPoints.Count; i++)
                MusicPoints.Add(new MusicPoint(model.musicPoints[i], World));

            PoetryPoints = new List<PoetryPoint>();
            for (int i = 0; i < model.poetryPoints.Count; i++)
                PoetryPoints.Add(new PoetryPoint(model.poetryPoints[i], World));

            GravityPoints = new List<GravityPoint>();
            for (int i = 0; i < model.gravityPoints.Count; i++)
                GravityPoints.Add(new GravityPoint(model.gravityPoints[i], this));

            FinalPoints = new List<Block>();
            for (int i = 0; i < model.finalPoints.Count; i++)
                FinalPoints.Add(new Block(model.finalPoints[i], World));

            AllComps = new List<DrawableBase>();

            AllComps.Add(Background);
            AllComps.Add(Platman);
            AllComps.AddRange(Blocks);
            AllComps.AddRange(DeathBlocks);
            AllComps.AddRange(TextPoints);
            AllComps.AddRange(MusicPoints);
            AllComps.AddRange(PoetryPoints);
            AllComps.AddRange(GravityPoints);
            AllComps.AddRange(FinalPoints);

            Platman.Visible = false;
        }

        private void _Constructor()
        {
            Platman.Camera.Position = Camera.Position;

            for (int i = 0; i < Blocks.Count; i++)
                Blocks[i].UpdateOrder = (int)LevelUpdateOrder.Blocks;

            UpdateOrder = (int)LevelUpdateOrder.Level;

            Platman.UpdateOrder = (int)LevelUpdateOrder.Person;
        }

        private List<BaseEffect> LoadShowPlatmanAnimation()
        {
            FadeEffect fade = new FadeEffect(false, 100, 1, 0, false, false);
            DelayEffect delay = new DelayEffect(800);
            FadeEffect fadeEffect = new FadeEffect(false, 100, 1, 0, false, false);
            fadeEffect.IsFocus = true;

            var list = new List<BaseEffect>();
            list.Add(fade);
            list.Add(delay);
            list.Add(fadeEffect);

            return list;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));

            /* IsColideTextPoint();
               IsColideMusicPoint();
               IsColideDeathBlock();
               IsColidePoetryPoint();
               IsColideGravityPoint();
               IsColideFinalPoint();
               */
            /*   if (PlayerInput.Instance.IsKeyDown(GameKey.Left))
               {
                   Camera.Move(new Vector2(-5, 0));
               }
               if (PlayerInput.Instance.IsKeyDown(GameKey.Right))
               {
                   Camera.Move(new Vector2(5, 0));
               }
          if (PlayerInput.Instance.IsKeyDown(GameKey.Down))
               {
                   Camera.Move(new Vector2(0, 5));
               }
               if (PlayerInput.Instance.IsKeyDown(GameKey.Up))
               {
                   Camera.Move(new Vector2(0, -5));
               }
               */

            Camera.Position = Platman.DrawPosition;
        }
       
        private void Platman_Dead(object sender)
        {
            ScreenParent.ResetLevel();
        }

        private void Platman_Showed(object sender)
        {
            PlayerInput.Instance.Enabled = true;
            Enabled = true;
        }

        private void Level_Finished(object sender)
        {
            List<BaseEffect> effects = new List<BaseEffect>();
            FadeEffect fade = new FadeEffect(true, 150, 1, 0);
            fade.IsFocus = false;
            effects.Add(fade);
            new GameAnimation(Camera, UpdateNextLevelAnimation, effects).Finished += NextLevel;
        }

        private void NextLevel(object sender)
        {
            ScreenParent.ResetLevel();
        }     
    }


    public partial class Level
    {
        public override void Unload()
        {
            base.Unload();

            for (int i = 0; i < AllComps.Count; i++)
                AllComps[i].Unload();

            Camera.SetPosition(cameraStartPos);
        }

        public void IsColideMusicPoint()
        {
            for (int i = 0; i < MusicPoints.Count; i++)
            {
                if (Platman.DrawBounds.Intersects(MusicPoints[i].DrawBounds))
                {
                    MusicPoints[i].Play();
                    return;
                }
            }
        }

        public void IsColideDeathBlock()
        {
            for (int i = 0; i < DeathBlocks.Count; i++)
            {
                if (Platman.DrawBounds.Intersects(DeathBlocks[i].DrawBounds))
                {
                    if (!DeathBlocks[i].Colided)
                    {
                        PlayerInput.Instance.Enabled = false;
                        DeathBlocks[i].Colided = true;
                        new GameAnimation(GameAnimationPredefined.Dead, UpdateDeathPlatmanAnimation).Finished += Platman_Dead;
                        return;
                    }
                }
            }

        }

        public void IsColideTextPoint()
        {
            for (int i = 0; i < TextPoints.Count; i++)
            {
                if (Platman.DrawBounds.Intersects(TextPoints[i].DrawBounds))
                {
                    TextBalloon instance = new TextBalloon(TextPoints[i].Text, Platman, World);
                    instance.Finished += TextBallon_Finished;

                    AllComps.Add(instance);
                    TextPoints.RemoveAt(i);
                    return;
                }
            }
        }

        private void IsColidePoetryPoint()
        {
            for (int i = 0; i < PoetryPoints.Count; i++)
            {
                if (Platman.DrawBounds.Intersects(PoetryPoints[i].DrawBounds))
                {
                    PoetryPoints[i].ShowPoetry(this);
                    PoetryPoints.RemoveAt(i);
                    return;
                }
            }
        }

        private void IsColideFinalPoint()
        {
            for (int i = 0; i < FinalPoints.Count; i++)
            {
                if (Platman.DrawBounds.Intersects(FinalPoints[i].DrawBounds))
                {
                    GravityManager.GravityBlocked = true;
                    GravityManager.Enabled = false;

                    FinalPoints.Remove(FinalPoints[i]);
                    new GameAnimation(GameAnimationPredefined.NextLevel, UpdateFinishLevelAnimation).Finished += Level_Finished;
                }
            }
        }
    }

    public sealed partial class Level
    {
        float previous = 0;

        private void TextBallon_Finished(object sender, EventArgs e)
        {
            TextBalloon b = (TextBalloon)sender;

            if (AllComps.Contains(b))
                AllComps.Remove(b);
        }

        private void UpdateDeathPlatmanAnimation(object sender)
        {
            EffectManager effect = (EffectManager)sender;


            if (previous != effect.Alpha)
            {
                previous = effect.Alpha;
            }


            ScreenParent.Alpha = 1 - effect.Alpha;
        }

        private void UpdateShowPlatmanAnimation(object sender)
        {
            EffectManager effect = (EffectManager)sender;

            Platman.Alpha = effect.Alpha;
        }

        private void UpdateFinishLevelAnimation(object sender)
        {
            EffectManager effect = (EffectManager)sender;

            Alpha = effect.Alpha;

            Platman.Alpha = 1f;
        }

        private void UpdateNextLevelAnimation(object sender)
        {
            EffectManager effect = (EffectManager)sender;

            Platman.Alpha = effect.Alpha;
        }
    }

}
