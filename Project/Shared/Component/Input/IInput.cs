namespace Platman.Component.Input
{
    public interface IInput
    {
        bool IsConnected { get; }
        GameKey[] PressedKeys { get; }
        void ClearInput();
    }
}
