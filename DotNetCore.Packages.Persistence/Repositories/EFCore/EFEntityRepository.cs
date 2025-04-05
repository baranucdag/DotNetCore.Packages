using System.Linq.Expressions;
using DotNetCore.Packages.Domain.Repositories.EFCore;
using DotNetCore.Packages.Domain.UnitOfWork;
using DotNetCore.Packages.Persistence.UnitOfWork;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.Packages.Persistence.Repositories.EFCore;

public class EFEntityRepository<TEntity, TContext> : IEntityRepository<TEntity>
    where TEntity : class where TContext : DbContext
{
    protected TContext Context { get; }
    private readonly IUnitOfWork _unitOfWork;

    public EFEntityRepository(TContext context, IUnitOfWork unitOfWork)
    {
        Context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await Context.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync();
        return entity;
    }

    public async Task<List<TEntity>> BulkAddAsync(List<TEntity> entities)
    {
        await Context.BulkInsertAsync(entities);
        await _unitOfWork.SaveChangesAsync();
        return entities;
    }

    public TEntity Update(TEntity entity)
    {
        Context.Update(entity);
        Context.SaveChanges();
        return entity;
    }

    public async Task<List<TEntity>> BulkUpdateAsync(List<TEntity> entities)
    {
        await Context.BulkUpdateAsync(entities);
        await _unitOfWork.SaveChangesAsync();
        return entities;
    }

    public TEntity Delete(TEntity entity)
    {
        Context.Remove(entity);
        Context.SaveChanges();
        return entity;
    }

    public async Task<List<TEntity>> BulkDeleteAsync(List<TEntity> entities)
    {
        await Context.BulkDeleteAsync(entities);
        await _unitOfWork.SaveChangesAsync();
        return entities;
    }

    public async Task<IEnumerable<TEntity>> GetListAsync(CancellationToken cancellationToken = default,
        Expression<Func<TEntity, bool>>? expression = null)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

        if (expression != null)
            query = query.Where(expression);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
    {
        return await Context.Set<TEntity>().FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default,
        Expression<Func<TEntity, bool>> expression = null)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

        if (expression != null)
            query = query.Where(expression);

        return await query.CountAsync(cancellationToken);
    }

    public Task<int> SaveChangesAsync()
    {
        return Context.SaveChangesAsync();
    }

    public bool Any(Expression<Func<TEntity, bool>> expression)
    {
        return Context.Set<TEntity>().Any(expression);
    }

    public IQueryable<TEntity> Query()
    {
        return Context.Set<TEntity>();
    }
}