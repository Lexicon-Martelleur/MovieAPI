using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCardAPI.Model.Repository;

public interface IBaseRepository
{
    Task<bool> CommitTransaction(IEnumerable<Func<Task>> actions);

    Func<Task> AsAsync(Action syncAction);

}
