using CRUD;
using Tiny.Models;

namespace Tiny.DTOs
{
    public class ProductDto : BaseDto, IMapFrom<Product>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }


    public class ProductFilterDto
    {

        public int? Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
    }
}
