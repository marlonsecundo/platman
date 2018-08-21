using System.IO;

namespace Platman.DataBase
{
    public interface IPlatformAdapter
    {
        void Exit();
        Stream GetInputStream(string name);
        Stream GetOutputStream(string name); 
    }
}
