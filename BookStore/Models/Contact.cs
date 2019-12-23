using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Contact
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Please Enter Valid Email Address: Example shamilov@outlook.com")]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
