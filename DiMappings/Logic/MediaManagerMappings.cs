using ExtendedIO;
using Ninject.Modules;

namespace DiMappings.Logic
{
    class MediaManagerMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<IMediaManager>().To<MediaManager>();
        }
    }
}
