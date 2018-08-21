using System.IO;
using System.IO.IsolatedStorage;
using Platman;
using Platman.DataBase;

namespace Desktop
{
    public class DesktopData : IPlatformAdapter
    {
        GameRoot game;

        public DesktopData(GameRoot game)
        {
            this.game = game;
        }

        public void Exit()
        {
            game.Exit();
        }

        public Stream GetInputStream(string name)
        {
            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForDomain();

            IsolatedStorageFileStream stream = new IsolatedStorageFileStream(name, FileMode.Create, file);

            return stream;
        }

        public Stream GetOutputStream(string name)
        {
            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForDomain();

            IsolatedStorageFileStream isoStream = file.OpenFile(name, FileMode.OpenOrCreate);

            return isoStream;

        }
    }
}
