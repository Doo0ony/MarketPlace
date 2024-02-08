using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models
{
    public class Category
    {
        [Key] public int CategoryId { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string CategoryThumbnail { get; set; }
        public List<SubCategory>? SubCategories { get; set; }
    }
}
