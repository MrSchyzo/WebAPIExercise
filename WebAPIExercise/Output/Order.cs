using System;
using System.Collections.Generic;

namespace WebAPIExercise.Output
{
    /// <summary>
    /// Output POCO for Order.
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CompanyCode { get; set; }
        public double Total { get; set; }
        public ICollection<OrderItem> OrderedProducts { get; set; }
    }
}
