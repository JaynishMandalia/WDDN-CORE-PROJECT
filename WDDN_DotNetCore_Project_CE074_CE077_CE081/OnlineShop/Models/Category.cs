using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, 
         MinLength(2, ErrorMessage ="Minimum length require is 2"), 
         RegularExpression(@"^[a-zA-Z0-9-]+$", ErrorMessage = "No special character allowed")
        ]
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }

    }
}
