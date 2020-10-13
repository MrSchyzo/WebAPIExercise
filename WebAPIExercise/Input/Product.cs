using System.ComponentModel.DataAnnotations;

namespace WebAPIExercise.Input
{
    /// <summary>
    /// Input POCO for Product.
    /// </summary>
    public class Product
    {
        [Required]
        [StringLength(maximumLength: 127, MinimumLength = 5)]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 2047, MinimumLength = 32)]
        public string Description { get; set; }
        [Required]
        [Range(0.0, 1_000.0)]
        public double UnitPrice { get; set; }
        [Required]
        [Range(1, 1_000_000_000)]
        public int StockQuantity { get; set; }
    }
}
