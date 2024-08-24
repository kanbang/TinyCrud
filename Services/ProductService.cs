using AutoMapper;
using CRUD;
using CRUD.Interfaces;
using Tiny.DTOs;
using Tiny.Models;


namespace Tiny.Services
{
    public class ProductService : BaseService<ProductDto, Product>
    {
        public ProductService(IBaseRepository<Product> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
