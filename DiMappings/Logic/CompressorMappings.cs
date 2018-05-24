using IO;
using Ninject.Modules;

namespace DiMappings.Logic
{
    public class CompressorMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<ICompressor>().To<Compressor>();
        }
    }
}
