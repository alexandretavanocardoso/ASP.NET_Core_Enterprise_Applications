using NSE.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NSE.Core.Data
{
    // Interface Geral
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {

    }
}
