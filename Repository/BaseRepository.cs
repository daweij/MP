using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Contracts;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Repository
{
  public class BaseRepository<T> : IRepository<T> where T : class, IEntity
  {
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(DbContext context)
    {
      if (context == null)
        throw new ArgumentNullException("context");

      _context = context;
      _dbSet = _context.Set<T>();
    }

    public BaseRepository(string connectionString)
    {
      if (String.IsNullOrWhiteSpace(connectionString))
        throw new ArgumentNullException("connectionString");

      _context = new MPContext(connectionString);
      _dbSet = _context.Set<T>();
    }


    public int Count(Expression<Func<T, bool>> predicate = null)
    {
      if (predicate != null)
        return _dbSet.Count(predicate);
      return _dbSet.Count();
    }

    public IEnumerable<T> Select(Expression<Func<T, bool>> predicate = null)
    {
      if (predicate != null)
        return _dbSet.Where(predicate);
      return _dbSet.AsQueryable();
    }
    
    public T Select(int id)
    {
      return _dbSet.Find(id);
    }
  }
}
