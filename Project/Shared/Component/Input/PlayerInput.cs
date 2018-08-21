using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Platman.Component.Managers;
using System.Linq;

namespace Platman.Component.Input
{
    public enum GameKey : int
    {
        Enter = 0,
        Back = 1,
        A = 2,
        B = 3,
        X = 4,
        Y = 5,
        Left = 6,
        Right = 7,
        Up = 8,
        Down = 9,
        LT = 10,
        RT = 11,
        Tab = 12,
        Esc = 13,
        TouchLeftPressed = 14,
        TouchRightPressed = 15,
        TouchLeftReleased = 16,
        TouchRightReleased = 17,
        None = 99
    }

    public partial class PlayerInput : GameComponent
    {
        public GameKey[] keysDown;

        private bool isSleep;
        private int timeSleep;
        private int time;

        public new bool Enabled
        {
            get => base.Enabled;
            set
            {
                keysDown = new GameKey[] { };
                touch.ClearInput();
                mouse.ClearInput();
                inputS.ClearInput();
                keyboard.ClearInput();
                gamePad.ClearInput();
                base.Enabled = value;
            }
        }

        private MouseInput mouse;
        private TouchInput touch;
        private InputScreen inputS;
        private KeyboardInput keyboard;
        private GamePadInput gamePad;

        DType deviceType;
        IInput input;

        private PlayerInput() : base(GameRoot.Instance)
        {
            keysDown = new GameKey[] { };

            gamePad = GamePadInput.Instance;
            keyboard = KeyboardInput.Instance;
            mouse = MouseInput.Instance;
            touch = TouchInput.Intance;
            inputS = InputScreen.Instance;

            deviceType = Device.Instance.DeviceType;
            ChangePreferedInput();

            Game.Components.Add(this);
        }

        public void SleepInput(int amount)
        {
            isSleep = true;
            timeSleep = amount;
            keysDown = new GameKey[] { };
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!isSleep)
                keysDown = GetPlayerInput();

            else
            {
                time += gameTime.ElapsedGameTime.Milliseconds;

                if (time > timeSleep)
                {
                    time = 0;
                    isSleep = false;
                }
            }
        }

        private GameKey[] GetPlayerInput()
        {
            GameKey[] keys = new GameKey[] { };

            keys = input.PressedKeys;

            if (keys.Length > 0 && mouse.Visible)
                mouse.Disable();

            return keys;
        }

        public void ChangePreferedInput()
        {
            IInput input = null;

            switch (deviceType)
            {
                case DType.Android:
                case DType.IOS:
                case DType.Win10_Phone:
                    input = inputS;
                    break;
                case DType.Desktop:
                case DType.Win10_PC:
                    if (gamePad.IsConnected) input = gamePad;
                    else if (keyboard.IsConnected) input = keyboard;
                    break;
                default:
                    input = keyboard;
                    break;
            }

            this.input = input; 
        }

        public bool IsKeyDown(GameKey key)
        {
            if (Enabled)
            {
                for (int i = 0; i < keysDown.Length; i++)
                {
                    if (keysDown[i] == key)
                    {
                        keysDown = new GameKey[] { };
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsComboKeyDown(GameKey[] keys)
        {
            if (Enabled)
            {
                if (!keys.Except(keysDown).Any() && keys.Length == keysDown.Length)
                {
                    keysDown = new GameKey[] { };
                    return true;
                }
            }
            return false;
        }

        public bool IsAnyKeyDown(GameKey[] keys)
        {
            if (Enabled)
            {
                if (keys.Intersect(keysDown).Any())
                {
                    keysDown = new GameKey[] { };
                    return true;
                }
            }

            return false;
        }

        public bool IsUnClearKeyDown(GameKey key)
        {
            if (Enabled)
            {
                for (int i = 0; i < keysDown.Length; i++)
                {
                    if (keysDown[i] == key)
                    {
                        return true;
                    }
                }
            }

            return false;
        }



        public bool IsMouseOver(Rectangle bounds)
        {
            if (!isSleep && Enabled)
            {
                Point pos = mouse.Position;
                Rectangle mouseBounds = new Rectangle(pos.X, pos.Y, 2, 2);

                if (bounds.Intersects(mouseBounds))
                    return true;
                else
                {
                    for (int i = 0; i < touch.Position.Length; i++)
                    {
                        mouseBounds = new Rectangle(touch.Position[i].X - 10, touch.Position[i].Y - 10, 20, 20);
                        if (bounds.Intersects(mouseBounds))
                            return true;
                    }
                }
            }

            return false;
        }

        public bool IsMouseClick(Rectangle bounds)
        {
            if (IsMouseOver(bounds))
            {
                if (mouse.PressedKeys[0] == GameKey.TouchLeftPressed)
                {
                    mouse.ClearInput();
                    return true;
                }
                else
                {
                    if (touch.PressedKeys[0] == GameKey.TouchLeftPressed)
                    {
                        touch.ClearInput();
                        return true;

                    }

                }
            }
            return false;
        }
    }


    public partial class PlayerInput : GameComponent
    {
        private static PlayerInput _instance;

        public static PlayerInput Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PlayerInput();


                return _instance;
            }
        }
    }
}
