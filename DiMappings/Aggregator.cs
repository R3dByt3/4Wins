using DiMappings.Logic;
using Ninject.Modules;

namespace DiMappings
{
    public class Aggregator
    {
        public INinjectModule[] Mappings => new INinjectModule[]
        {
            new LoggerMappings(),
            new DataAccessMappings(),
            new ConfiguratorMappings()
        };
    }
}
