using System;
using System.Collections.Generic;

namespace WebAPIExercise.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CompanyCode { get; set; }
        public IEnumerable<OrderItem> OrderItems {get; set;}
    }
}
