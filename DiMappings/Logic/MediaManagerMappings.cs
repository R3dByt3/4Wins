using IO;
using Ninject.Modules;

namespace DiMappings.Logic
{
    public class MediaManagerMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<IMediaManager>().To<MediaManager>();
        }
    }
}
