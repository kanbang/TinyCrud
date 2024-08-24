using System.Reflection;
using CRUD;
using Microsoft.EntityFrameworkCore;
using Tiny.Models;

namespace Tiny.Data
{
    public class TinyDbContext : DbContext
    {
        public TinyDbContext(DbContextOptions<TinyDbContext> options) : base(options)
        {
        }

        // public DbSet<Product> Products { get; set; }
        // Add other DbSets as needed

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 获取当前程序集中所有继承自 BaseEntity 的类型
            var entityTypes = Assembly.GetExecutingAssembly().GetTypes()
                                      .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseEntity)));

            foreach (var entityType in entityTypes)
            {
                modelBuilder.Model.AddEntityType(entityType);
            }
        }
    }
}
