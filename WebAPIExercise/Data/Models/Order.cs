using System;
using System.Collections.Generic;

namespace WebAPIExercise.Data.Models
{
    /// <summary>
    /// DB Entity for Order.
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CompanyCode { get; set; }
        public ICollection<OrderItem> OrderItems {get; set;}
    }
}
