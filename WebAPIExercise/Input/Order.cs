﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPIExercise.Input
{
    /// <summary>
    /// Input POCO for Order.
    /// </summary>
    public class Order
    {
        [Required]
        public string CompanyCode { get; set; }
        [Required]
        [MinLength(1)]
        public IEnumerable<OrderItem> Items { get; set; }
    }
}
