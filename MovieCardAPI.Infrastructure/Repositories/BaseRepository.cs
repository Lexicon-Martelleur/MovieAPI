using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Model.Repository;

namespace MovieCardAPI.Infrastructure.Repositories;

public class BaseRepository<EntityType> : IBaseRepository
     where EntityType : class
{
    protected MovieContext Context { get; }

    protected DbSet<EntityType> DbSet { get; }

    public BaseRepository(MovieContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        DbSet = context.Set<EntityType>();
    }

    public async Task<bool> CommitTransaction(
        IEnumerable<Func<Task>> dbActions)
    {
        using var transaction = await Context.Database.BeginTransactionAsync();
        try
        {
            foreach (var action in dbActions)
            {
                action?.Invoke();
                await Context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public Func<Task> AsAsync(Action syncAction)
    {
        return () =>
        {
            syncAction();
            return Task.CompletedTask;
        };
    }
}
