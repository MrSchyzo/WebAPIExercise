namespace WebAPIExercise.Input
{
    /// <summary>
    /// Input POCO for OrderItem.
    /// </summary>
    public class OrderItem
    {
        public int ProductId { get; set; }
        public int OrderedQuantity { get; set; }
    }
}