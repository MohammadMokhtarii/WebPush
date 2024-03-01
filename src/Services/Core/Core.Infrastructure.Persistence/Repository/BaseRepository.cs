namespace Core.Infrastructure.Persistence.Repository;

public abstract class BaseRepository(ApplicationDbContext context)
{
    protected readonly ApplicationDbContext _context = context;
}
