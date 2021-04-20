using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Core.Data
{
    // Repositorio usado apenas para ter o commit
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
