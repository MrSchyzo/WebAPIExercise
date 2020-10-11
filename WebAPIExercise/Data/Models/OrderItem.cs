using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPIExercise.Data.Models
{
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
