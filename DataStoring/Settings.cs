using DataStoring.Contracts;
using ExtendedIO.SQLiteSupport;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace DataStoring
{
    public class Settings : BaseNode, ISettings
    {
        [TextBlob("Example")]
        public IList<string> Example { get; set; }
    }
}
