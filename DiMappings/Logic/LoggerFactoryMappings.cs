using Logger;
using Ninject.Modules;
using static Logger.LoggingLevelEnum;

namespace DiMappings.Logic
{
    internal class LoggerFactoryMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<ILoggerFactory>().To<LoggerFactory>().WithConstructorArgument("GUI").WithConstructorArgument(LoggingLevel.Debug);
        }
    }
}
