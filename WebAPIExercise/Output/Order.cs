﻿using System;
using System.Collections.Generic;

namespace WebAPIExercise.Output
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CompanyCode { get; set; }
        public double Total { get; set; }
        public IEnumerable<ProductItem> Items { get; set; }
    }
}