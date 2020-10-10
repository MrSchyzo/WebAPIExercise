using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPIExercise.Input
{
    public class Order
    {
        [Required]
        public string CompanyCode { get; set; }
        [Required]
        [MinLength(1)]
        public IEnumerable<ProductItem> Items { get; set; }
    }
}
