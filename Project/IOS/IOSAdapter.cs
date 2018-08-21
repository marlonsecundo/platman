using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using Platman.DataBase;

namespace IOS
{
    public class IOSAdapter : IPlatformAdapter
    {
        public void Exit()
        {
            Thread.CurrentThread.Abort();
        }

        public Stream GetInputStream(string name)
        {
            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();

            IsolatedStorageFileStream stream = new IsolatedStorageFileStream(name, FileMode.Create, file);

            return stream;
        }

        public Stream GetOutputStream(string name)
        {
            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();

            IsolatedStorageFileStream isoStream = file.OpenFile(name, FileMode.OpenOrCreate);

            return isoStream;

        }
    }
}