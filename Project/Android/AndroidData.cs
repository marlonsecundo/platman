using Platman.DataBase;
using System.IO;
using System.IO.IsolatedStorage;

namespace Android
{
    public class AndroidData : IPlatformAdapter
    {
        Activity1 activity;
        public AndroidData(Activity1 mainActivity)
        {
            activity = mainActivity;
        }
        public void Exit()
        {
            activity.Finish();
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