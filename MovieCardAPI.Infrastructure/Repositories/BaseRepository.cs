using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Model.Repository;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MovieCardAPI.Infrastructure.Repositories;

public class BaseRepository<EntityType> : IBaseRepository<EntityType>
     where EntityType : class
{
    protected MovieContext Context { get; }

    protected DbSet<EntityType> ThisDbSet { get; }

    public BaseRepository(MovieContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        ThisDbSet = context.Set<EntityType>();
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
            syncAction?.Invoke();
            return Task.CompletedTask;
        };
    }

    public IQueryable<EntityType> FindAll(bool trackChanges = true) => !trackChanges
        ? ThisDbSet.AsNoTracking()
        : ThisDbSet;

    public IQueryable<EntityType> FindByCondition(
        Expression<Func<EntityType, bool>> expression,
        bool trackChanges = true) => !trackChanges
        ? ThisDbSet.Where(expression).AsNoTracking()
        : ThisDbSet.Where(expression);
}
