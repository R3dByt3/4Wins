using DiMappings.Logic;
using Ninject.Modules;

namespace DiMappings
{
    public class Aggregator
    {
        public INinjectModule[] Mappings => new INinjectModule[]
        {
            new LoggerFactoryMappings(),
            new FileManagerMappings(),
            new MediaManagerMappings(),
            new CompressorMappings(),
            new SettingsMappings(),
            new DBAccessMappings(),
            new ConfiguratorMappings()
        };
    }
}
