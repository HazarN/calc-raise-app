using CalcRaise.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CalcRaise.Infrastructure.Persistence.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly CalcRaiseDbContext _context;

    public Repository(CalcRaiseDbContext context)
    {
        _context = context;
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.Set<TEntity>().FindAsync(new object[] { id }, ct);

    public async Task<List<TEntity>> ListAsync(CancellationToken ct = default) =>
        await _context.Set<TEntity>().ToListAsync(ct);

    public async Task AddAsync(TEntity entity, CancellationToken ct = default) =>
        await _context.Set<TEntity>().AddAsync(entity, ct);

    public void Remove(TEntity entity) => _context.Set<TEntity>().Remove(entity);
}
