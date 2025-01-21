// Repositories/GenericRepository.cs
using Hahn.Data.Context;
using Hahn.Data.Interfaces.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hahn.Data.Repositories.BaseRepository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly HahnDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(HahnDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    public virtual async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<bool> RemoveAsync(T entity)
    {
        _dbSet.Remove(entity);
        return true;
    }

    public virtual async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

