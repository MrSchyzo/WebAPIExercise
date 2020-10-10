using System;

namespace WebAPIExercise.Output
{
    public class OrderSummary
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CompanyCode { get; set; }
        public double Total { get; set; }
    }
}
