using System;
using System.Collections.Generic;

namespace Configuration.Contracts
{
    public interface IConfigurator
    {
        void Set<T>(T Setting) where T : class;
        T Get<T>() where T : class;
    }
}
