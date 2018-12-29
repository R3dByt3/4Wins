using Logger;
using Ninject.Modules;
using static Logger.LoggingLevelEnum;

namespace DiMappings.Logic
{
    internal class LoggerMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<ILoggerFactory>().To<LoggerFactory>().WithConstructorArgument("GUI", LoggingLevel.Debug).WithConstructorArgument(LoggingLevelEnum.LoggingLevel.Debug);
        }
    }
}
