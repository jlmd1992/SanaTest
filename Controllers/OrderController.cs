using Microsoft.AspNetCore.Mvc;
using SanaTest.Model;

namespace SanaTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder([FromBody] CreateOrder orderModel)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    // Crear una nueva orden
                    var order = new Order
                    {
                        Code = GenerateRandomCode(), // Generar un código aleatorio para la orden
                        CustomerId = orderModel.CustomerId,
                        Date = DateTime.Now
                    };
                    _context.Order.Add(order);
                    await _context.SaveChangesAsync();

                    // Recorrer los productos en el carrito y agregarlos a OrderProducts
                    foreach (var item in orderModel.Products)
                    {
                        var product = await _context.Product.FindAsync(item.ProductId);
                        if (product != null)
                        {
                            var orderProduct = new OrderProduct
                            {
                                OrderId = order.OrderId,
                                ProductId = item.ProductId,
                                QuantityProducts = item.Quantity,
                                Total = item.Quantity * product.Price
                            };
                            _context.OrderProducts.Add(orderProduct);

                            // Reducir la cantidad de stock del producto en la base de datos
                            product.Stock -= item.Quantity;
                        }
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // Método para generar un código aleatorio para la orden
        private string GenerateRandomCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
