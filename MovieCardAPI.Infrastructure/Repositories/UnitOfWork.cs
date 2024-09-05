using Microsoft.EntityFrameworkCore.Storage;
using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Model.Repository;
using System.Xml;

namespace MovieCardAPI.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MovieContext _context;
    
    private readonly Lazy<IMovieRepository> _movieRepository;

    public IMovieRepository MovieRepository => _movieRepository.Value;

    public UnitOfWork(MovieContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _movieRepository = new Lazy<IMovieRepository>(() => new MovieRepository(context));
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }

    public async Task<bool> ExecuteAndSaveTransaction(IEnumerable<Func<Task>> dbChangeActions)
    {
        var isSaved = false;
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var action in dbChangeActions)
            {
                action?.Invoke();
                isSaved = await SaveChangesAsync();
            }

            await transaction.CommitAsync();
            return isSaved;
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