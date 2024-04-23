using Microsoft.AspNetCore.Mvc;
using SanaTest.Model;
using Microsoft.EntityFrameworkCore;

namespace SanaTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int page = 1, int pageSize = 10)
        {
            var products = await _context.Product
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return products;
        }

        // GET: api/Product/CheckStock/{id}
        [HttpGet("CheckStock/{productId}/{quantity}")]
        public async Task<ActionResult<bool>> CheckStock([FromRoute] int productId, [FromRoute] int quantity)
        {
            var product = await _context.Product.FindAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            // Check if stock is enought
            bool isAvailable = product.Stock > quantity;

            return isAvailable;
        }
    }
}
