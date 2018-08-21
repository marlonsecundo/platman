using Platman.DataBase;

namespace Platman
{
    public enum DType
    {
        Android,
        IOS,
        Desktop,
        Win10_Phone,
        Win10_PC
    }
    public class Device
    {
        private static Device _instance;
        public static Device Instance => _instance;
        public DType DeviceType { get; }
        public IPlatformAdapter PlatData { get; }
        private Device(DType type, IPlatformAdapter plat)
        {
            DeviceType = type;
            PlatData = plat;
        }

        public static void Init(DType type, IPlatformAdapter plat)
        {
            if (_instance == null)
                _instance = new Device(type, plat);
        }
    }
}
