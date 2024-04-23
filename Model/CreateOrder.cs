using System.ComponentModel.DataAnnotations;

namespace SanaTest.Model
{
    public class CreateOrder
    {
        public int CustomerId { get; set; }
        public List<OrderProductModel> Products { get; set; }
    }

    public class OrderProductModel
    {
        [Key]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
