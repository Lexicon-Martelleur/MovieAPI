namespace MovieCardAPI.Model.Repository;

public interface IBaseRepository
{
    Task<bool> CommitTransaction(IEnumerable<Func<Task>> actions);

    Func<Task> AsAsync(Action syncAction);
}
