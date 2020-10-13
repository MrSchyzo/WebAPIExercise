namespace WebAPIExercise.Output
{
    /// <summary>
    /// Output POCO for OrderItem.
    /// </summary>
    public class OrderItem
    {
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public int OrderedQuantity { get; set; }
    }
}