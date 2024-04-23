using System.ComponentModel.DataAnnotations;

namespace SanaTest.Model
{
    public class OrderProduct
    {
        [Key]
        public int OrderProductId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int QuantityProducts { get; set; }
        public decimal Total { get; set; }

        // Relación con la tabla Order
        public Order Order { get; set; }
    }
}
