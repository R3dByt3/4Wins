using ExtendedIO;
using Ninject.Modules;

namespace DiMappings.Logic
{
    class CompressorMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<ICompressor>().To<Compressor>();
        }
    }
}
