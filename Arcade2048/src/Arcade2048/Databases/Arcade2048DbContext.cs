namespace Arcade2048.Databases;

using Arcade2048.Domain;
using Arcade2048.Databases.EntityConfigurations;
using Arcade2048.Services;
using Arcade2048.Exceptions;
using Resources;
using MediatR;
using Arcade2048.Domain.Users;
using Arcade2048.Domain.ExternalLogins;
using Arcade2048.Domain.Scores;
using Arcade2048.Domain.LeaderboardEntries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

public sealed class Arcade2048DbContext(DbContextOptions<Arcade2048DbContext> options, 
    ICurrentUserService currentUserService, 
    IMediator mediator, 
    TimeProvider dateTimeProvider)
    : DbContext(options)
{
    #region DbSet Region - Do Not Delete
    public DbSet<User> Users { get; set; }
    public DbSet<ExternalLogin> ExternalLogins { get; set; }
    public DbSet<Score> Scores { get; set; }
    public DbSet<LeaderboardEntry> LeaderboardEntries { get; set; }
    #endregion DbSet Region - Do Not Delete

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.FilterSoftDeletedRecords();
        /* any query filters added after this will override soft delete 
                https://docs.microsoft.com/en-us/ef/core/querying/filters
                https://github.com/dotnet/efcore/issues/10275
        */

        #region Entity Database Config Region - Only delete if you don't want to automatically add configurations
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ExternalLoginConfiguration());
        modelBuilder.ApplyConfiguration(new ScoreConfiguration());
        modelBuilder.ApplyConfiguration(new LeaderboardEntryConfiguration());
        #endregion Entity Database Config Region - Only delete if you don't want to automatically add configurations
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        var result = base.SaveChanges();
        _dispatchDomainEvents().GetAwaiter().GetResult();
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateAuditFields();
        var result = await base.SaveChangesAsync(cancellationToken);
        await _dispatchDomainEvents();
        return result;
    }
    
    private async Task _dispatchDomainEvents()
    {
        var domainEventEntities = ChangeTracker.Entries<BaseEntity>()
            .Select(po => po.Entity)
            .Where(po => po.DomainEvents.Any())
            .ToArray();

        foreach (var entity in domainEventEntities)
        {
            var events = entity.DomainEvents.ToArray();
            entity.DomainEvents.Clear();
            foreach (var entityDomainEvent in events)
                await mediator.Publish(entityDomainEvent);
        }
    }
        
    private void UpdateAuditFields()
    {
        var now = dateTimeProvider.GetUtcNow();
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.UpdateCreationProperties(now, currentUserService?.UserId);
                    entry.Entity.UpdateModifiedProperties(now, currentUserService?.UserId);
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdateModifiedProperties(now, currentUserService?.UserId);
                    break;
                
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.UpdateModifiedProperties(now, currentUserService?.UserId);
                    entry.Entity.UpdateIsDeleted(true);
                    break;
            }
        }
    }
}

public static class Extensions
{
public static void FilterSoftDeletedRecords(this ModelBuilder modelBuilder)
{
    Expression<Func<BaseEntity, bool>> filterExpr = e => !e.IsDeleted;
    foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes()
        .Where(m => m.ClrType.IsAssignableTo(typeof(BaseEntity))))
    {
        // modify expression to handle correct child type
        var parameter = Expression.Parameter(mutableEntityType.ClrType);
        var body = ReplacingExpressionVisitor
            .Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
        var lambdaExpression = Expression.Lambda(body, parameter);

        // set filter
        mutableEntityType.SetQueryFilter(lambdaExpression);
    }
}

    public static async Task<TEntity> GetByIdOrDefault<TEntity>(this DbSet<TEntity> dbSet, 
        Guid id, 
        CancellationToken cancellationToken = default) 
            where TEntity : BaseEntity
    {
        return await dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
    
    public static async Task<TEntity> GetByIdOrDefault<TEntity>(this IQueryable<TEntity> query, 
        Guid id, 
        CancellationToken cancellationToken = default) 
            where TEntity : BaseEntity
    {
        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    } 
    
    public static async Task<TEntity> GetById<TEntity>(this DbSet<TEntity> dbSet, 
        Guid id, 
        CancellationToken cancellationToken = default) 
            where TEntity : BaseEntity
    {
        var result = await dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    
        return result.MustBeFoundOrThrow();
    }
    
    public static async Task<TEntity> GetById<TEntity>(this IQueryable<TEntity> query, 
        Guid id, 
        CancellationToken cancellationToken = default) 
            where TEntity : BaseEntity
    {
        var result = await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    
        return result.MustBeFoundOrThrow();
    }

    public static TEntity MustBeFoundOrThrow<TEntity>(this TEntity entity)
        where TEntity : BaseEntity
    {
         return entity ?? throw new NotFoundException($"{typeof(TEntity).Name} was not found.");
    }
}
