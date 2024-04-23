namespace SanaTest.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public string Code { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }

        // Relación con la tabla OrderProduct
        public List<OrderProductModel> OrderProducts { get; set; }
    }
}
