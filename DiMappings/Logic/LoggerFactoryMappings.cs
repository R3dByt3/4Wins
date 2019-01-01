using Logger;
using Ninject.Modules;
using static Logger.LoggingLevelEnum;

namespace DiMappings.Logic
{
    internal class LoggerFactoryMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<ILoggerFactory>().To<LoggerFactory>()
                .InSingletonScope()
                .WithConstructorArgument("Renter")
                .WithConstructorArgument(LoggingLevel.Debug);
        }
    }
}
