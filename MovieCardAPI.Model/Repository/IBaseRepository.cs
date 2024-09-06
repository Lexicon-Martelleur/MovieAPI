using System.Linq.Expressions;

namespace MovieCardAPI.Model.Repository;

public interface IBaseRepository<EntityType> where EntityType : class
{
    Task<bool> CommitTransaction(IEnumerable<Func<Task>> actions);

    Func<Task> AsAsync(Action syncAction);

    public IQueryable<EntityType> FindAll(bool trackChanges = true);

    public IQueryable<EntityType> FindByCondition(
        Expression<Func<EntityType, bool>> expression,
        bool trackChanges = true);
}
