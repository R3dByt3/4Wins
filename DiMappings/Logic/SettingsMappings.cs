using DataStoring;
using DataStoring.Contracts;
using Ninject.Modules;

namespace DiMappings.Logic
{
    public class SettingsMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<ISettings>().To<Settings>();
        }
    }
}
