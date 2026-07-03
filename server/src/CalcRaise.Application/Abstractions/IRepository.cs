namespace CalcRaise.Application.Abstractions;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<TEntity>> ListAsync(CancellationToken ct = default);
    Task AddAsync(TEntity entity, CancellationToken ct = default);
    void Remove(TEntity entity);
}
