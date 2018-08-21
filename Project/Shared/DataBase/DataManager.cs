using System.IO;
using System.Xml.Serialization;

namespace Platman.DataBase
{
    public enum Filename
    {
        Setting,
    }

    public partial class DataManager
    {
        private IPlatformAdapter platData;
        private DataManager()
        {
            platData = Device.Instance.PlatData;
        }

        public void Save<T>(Filename name, T data)
        {
            using (Stream stream = platData.GetInputStream(name + ".xml"))
            {
                XmlSerializer xml = new XmlSerializer(data.GetType());
                xml.Serialize(stream, data);
                stream.Flush();
            }
        }
        public T Load<T>(Filename name)
        {
            using (Stream stream = platData.GetOutputStream(name + ".xml"))
            {
                if (stream.Length > 50)
                {
                    XmlSerializer xml = new XmlSerializer(typeof(T));
                    using (TextReader reader = new StreamReader(stream))
                        return (T)xml.Deserialize(reader);
                }
                else
                {
                    stream.Dispose();
                    var data = GameRoot.Instance.Content.Load<T>("Data/" + name);
                    Save<T>(name, data);
                    return data;
                }
            }
        }
    }

    public partial class DataManager
    {
        private static DataManager _instance;
        public static DataManager Instance => _instance = _instance ?? new DataManager();
    }
}
