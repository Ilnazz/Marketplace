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
            entity.Navigation(d => d.ProductCategory).AutoInclude();
            entity.Navigation(d => d.ProductManufacturer).AutoInclude();
        });
    }
}