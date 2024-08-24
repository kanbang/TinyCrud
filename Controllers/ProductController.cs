using CRUD;
using Tiny.DTOs;
using Tiny.Models;
using Tiny.Services;
using Microsoft.AspNetCore.Mvc;

namespace Tiny.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : BaseController<ProductDto, Product>
    {
        public ProductController(ProductService service) : base(service)
        {
        }
    }
}
