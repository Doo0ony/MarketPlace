using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models
{
    public class Product
    {
        [Key] public int ProductId { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string ProductThumbnail { get; set; }
        [Required] public double Cost { get; set; }
        [Required] public ushort Amount { get; set; }
        public string? AuthorId { get; set; }
        [Required] public int SubCategoryId { get; set; }
        public SubCategory? SubCategory { get; set; }
        public List<UserBasket>? UserBasket { get; set; }
    }
}
