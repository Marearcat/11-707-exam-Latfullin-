using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam2.Models
{
    public class Dish
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
    }
}
