namespace Core.Application.Common;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    public bool HasActiveTransaction();
    public Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    public Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    public void RollbackTransaction();
    public Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default);
}


public static class UnitOfWorkErrors
{
    public readonly static Error SaveChangesError = Error.Exception("SaveChangesError", "SaveChanges Error");
}