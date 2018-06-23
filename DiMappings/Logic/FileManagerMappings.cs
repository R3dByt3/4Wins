using ExtendedIO;
using Ninject.Modules;

namespace DiMappings.Logic
{
    class FileManagerMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<IFileManager>().To<FileManager>();
        }
    }
}
