using Logger;
using Ninject.Modules;

namespace DiMappings.Logic
{
    class LoggerMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<Logger.Logger>().WithConstructorArgument("GUI").WithConstructorArgument(LoggingLevelEnum.LoggingLevel.Debug);
        }
    }
}
