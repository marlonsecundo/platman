using System.IO;
using System.IO.IsolatedStorage;
using Platman.DataBase;
using Windows.UI.Xaml;

namespace Win10
{
    public class Win10Data : IPlatformAdapter
    {
        public void Exit()
        {
            Application.Current.Exit();
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
