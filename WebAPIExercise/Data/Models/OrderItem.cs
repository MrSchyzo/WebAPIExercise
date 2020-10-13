using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPIExercise.Data.Models
{
    /// <summary>
    /// DB Entity for OrderItem.
    /// </summary>
    public class OrderItem
    {
        public int Id { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int OrderedQuantity { get; set; }
    }
}
