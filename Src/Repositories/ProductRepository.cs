using CRUD;
using Microsoft.EntityFrameworkCore;
using Tiny.Data;
using Tiny.Models;

namespace Tiny.Repositories
{
    public class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository(TinyDbContext context) : base(context)
        {
        }
    }
}
