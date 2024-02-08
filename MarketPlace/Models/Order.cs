namespace MarketPlace.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
