using AutoMapper;
using Hahn.Data.Context;
using Hahn.Data.Interfaces.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace Hahn.Data.Repositories.BaseRepository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly HahnDbContext _context;
    protected readonly DbSet<T> _dbSet;
    private readonly IMapper _mapper;
    public GenericRepository(HahnDbContext context,IMapper mapper)
    {
        _context = context;
        _dbSet = _context.Set<T>();
        _mapper = mapper;
    }
    public virtual async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
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

    public async Task<IEnumerable<T>> SearchByTitleAsync(string title)
    {
        return await _dbSet
            .Where(x => EF.Property<string>(x, "Title").Contains(title))
            .ToListAsync();
    }

    public TDto MapToDto<TDto>(T entity) where TDto : class
    {
        return _mapper.Map<TDto>(entity);
    }

    public IEnumerable<TDto> MapToDtos<TDto, TEntity>(IEnumerable<TEntity> entities)
        where TDto : class
        where TEntity : class
    {
        return entities.Select(entity => _mapper.Map<TDto>(entity));
    }
}


