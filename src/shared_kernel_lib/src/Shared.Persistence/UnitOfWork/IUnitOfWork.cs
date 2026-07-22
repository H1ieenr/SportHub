
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Shared.Persistence
{
    public interface IUnitOfWork<out TContext> : IDisposable where TContext : DbContext
    {
        TContext Context { get; }
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        bool HasActiveTransaction { get; }
    }
}