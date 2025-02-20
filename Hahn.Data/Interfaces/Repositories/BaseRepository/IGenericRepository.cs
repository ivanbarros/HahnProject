using Hahn.Data.Dtos.Recipies;
using Hahn.Domain.Entities;

namespace Hahn.Data.Interfaces.Repositories.BaseRepository;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task<bool> RemoveAsync(T entity);
    Task SaveChangesAsync();
    Task<IEnumerable<T>> SearchByTitleAsync(string title);
    TDto MapToDto<TDto>(T entity) where TDto : class;
    IEnumerable<TDto> MapToDtos<TDto, TEntity>(IEnumerable<TEntity> entities) where TDto : class where TEntity : class;
}
