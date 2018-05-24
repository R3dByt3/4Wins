using IO;
using Ninject.Modules;

namespace DiMappings.Logic
{
    public class FileManagerMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<IFileManager>().To<FileManager>();
        }
    }
}
