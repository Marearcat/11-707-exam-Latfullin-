using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam2.Models
{
    public class DishToCart
    {
        [Key]
        public string Id { get; set; }
        public string DishId { get; set; }
        public string CartId { get; set; }
    }
}
