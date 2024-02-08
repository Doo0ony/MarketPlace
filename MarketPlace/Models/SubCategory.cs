using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models
{
    public class SubCategory
    {
        [Key] public int SubCategoryId { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string SubCategoryThumbnail { get; set; }
        [Required] public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<Product>? Products { get; set; }
    }
}
