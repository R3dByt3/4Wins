using ExtendedIO.SQLiteSupport;
using System.Collections.Generic;

namespace DataStoring.Contracts
{
    public interface ISettings : IBaseNode
    {
        IList<string> Example { get; set; }
    }
}
