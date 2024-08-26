using AutoMapper;
using CRUD;
using CRUD.Interfaces;
using Tiny.DTOs;
using Tiny.Models;
using Tiny.Repositories;


namespace Tiny.Services
{
    public class ProductService : BaseService<ProductDto, Product, ProductDto, ProductDto, ProductFilterDto>
    {
        public ProductService(ProductRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
