using System.Collections;

namespace WebAPIExercise.Data.Models
{
    /// <summary>
    /// DB Entity for Product.
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public int StockQuantity { get; set; }
    }
}
