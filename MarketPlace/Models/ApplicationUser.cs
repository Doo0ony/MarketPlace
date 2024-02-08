using Microsoft.AspNetCore.Identity;

namespace MarketPlace.Models
{
    public class ApplicationUser : IdentityUser 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Balance { get; set; } = 0.0;
        public string BasketId { get; set; }
        public UserBasket? UserBasket { get; set; }
        public List<Order>? Orders { get; set; }
    }
}
