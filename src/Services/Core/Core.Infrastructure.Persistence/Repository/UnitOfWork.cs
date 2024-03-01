using Core.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Common;

namespace Core.Infrastructure.Persistence.Repository;

public class UnitOfWork(ApplicationDbContext context, IPublisher publisher) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;
    protected readonly ApplicationDbContext _context = context;
    protected readonly IPublisher _publisher = publisher;

    public bool HasActiveTransaction()
        => _transaction is not null;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        => _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            await _transaction.CommitAsync(cancellationToken);
    }

    public void RollbackTransaction()
    {
        if (_transaction is not null)
            _transaction.Rollback();
    }

    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken)
    {

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception)
        {
            return UnitOfWorkErrors.SaveChangesError;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
        if (_transaction is not null)
            _transaction!.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        if (_transaction is not null)
            await _transaction!.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
