using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T?> GetByIdAsync(long entityId,CancellationToken cancellationToken);
        Task AddAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
    }
} 