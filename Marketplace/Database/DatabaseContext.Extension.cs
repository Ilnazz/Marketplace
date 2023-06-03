using System.Linq;
using Marketplace.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Database;

public partial class DatabaseContext
{
    private static readonly DatabaseContext _entities = null!;
    public static readonly DatabaseContext Entities = _entities ??= new();

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Navigation(d => d.ProductPhotos).AutoInclude();
            entity.Navigation(d => d.Salesman).AutoInclude();
            entity.Navigation(d => d.ProductPhotos).AutoInclude();
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.Navigation(d => d.Basket).AutoInclude();
            entity.Navigation(d => d.Orders).AutoInclude();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Navigation(d => d.OrderProducts).AutoInclude();
        });
    }

    public void CancelChanges()
    {
        foreach (var entry in Entities.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }
    }

    public void CancelChanges<TEntity>(TEntity entity) where TEntity : class
    {
        var entry = Entities.Entry(entity);

        switch (entry.State)
        {
            case EntityState.Modified:
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
                break;
            case EntityState.Added:
                entry.State = EntityState.Detached;
                break;
            case EntityState.Deleted:
                entry.State = EntityState.Unchanged;
                break;
        }
    }

    public void SaveChanges<TEntity>(TEntity entity) where TEntity : class
    {
        var entry = Entities.Entry(entity);
        entry.OriginalValues.SetValues(entry.CurrentValues);
    }

    public bool HasChanges() => Entities.ChangeTracker.HasChanges();

    public bool HasChanges<TEntity>(TEntity entity) where TEntity : class
    {
        var entry = Entities.Entry(entity);
        return new[] { EntityState.Modified, EntityState.Deleted, EntityState.Added }.Contains(entry.State);
    }
}