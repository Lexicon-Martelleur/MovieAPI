namespace MovieCardAPI.Model.Repository;

public interface IUnitOfWork
{
    IMovieRepository MovieRepository { get; }

    Task<bool> SaveChangesAsync();

    Task<bool> ExecuteAndSaveTransaction(IEnumerable<Func<Task>> actions);

    Func<Task> AsAsync(Action syncAction);
}
