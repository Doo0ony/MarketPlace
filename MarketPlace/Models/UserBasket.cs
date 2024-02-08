namespace MarketPlace.Models
{
    public class UserBasket
    {
        public int Id { get; set; }
        public string UserBasketId { get; set; }
        public int ProductId { get; set; }
        public List<Product>? UsersBasketProducts { get; set; }
    }
}
