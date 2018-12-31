using ExtendedIO.SQLiteSupport;
using Ninject.Modules;

namespace DiMappings.Logic
{
    internal class DBAccessMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<IDBAccess>().To<DBAccess>();
        }
    }
}
