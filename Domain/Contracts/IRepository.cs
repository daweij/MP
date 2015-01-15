using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
  public interface IRepository<T> where T: IEntity
  {
    T Select(int id);
    IEnumerable<T> Select(Expression<Func<T, bool>> predicate = null);
    int Count(Expression<Func<T, bool>> predicate = null);
  }
}
