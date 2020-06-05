using Caliburn.Micro;
using Configuration;
using Configuration.Contracts;
using DataStoring;
using DataStoring.Contracts;
using NetStandard.IO.Compression;
using NetStandard.IO.Files;
using NetStandard.Logger;
using Networking;
using Networking.Contracts;
using Ninject.Modules;
using System;

namespace DiMappings.Logic
{
    internal class Mappings : NinjectModule
    {
        public override void Load()
        {
            Bind<ILoggerFactory>().To<LoggerFactory>()
                .InSingletonScope()
                .WithConstructorArgument(AppDomain.CurrentDomain.FriendlyName)
                .WithConstructorArgument(LoggingLevel.Debug);
                //.OnDeactivation(x => x.Dispose());

            Bind<ICompressor>().To<Compressor>();
            Bind<IFileManager>().To<FileManager>();

            Bind<IConfigurator>().To<Configurator>();
            Bind<ISettings>().To<Settings>();

            Bind<IEventAggregator>().To<EventAggregator>()
                .InSingletonScope();

            Bind<IAsynchronousClient>().To<AsynchronousClient>()
                .InSingletonScope();

            Bind<IBroadCaster>().To<BroadCaster>();

            Bind<IServerSocket>().To<ServerSocket>()
                .InSingletonScope();

            Bind<ISocketPackage>().To<SocketPackage>();
        }
    }
}
