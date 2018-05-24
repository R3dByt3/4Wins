using IO;
using Ninject.Modules;

namespace DiMappings.Logic
{
    class DBAccessMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<IDBAccess>().To<DBAccess>();
        }
    }
}
