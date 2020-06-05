using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Contracts
{
    public interface ISocketPackage
    {
        RequestType RequestType { get; set; }
        int Value { get; set; }
    }
}
