using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Interface.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CompleteAsync();
    }
}