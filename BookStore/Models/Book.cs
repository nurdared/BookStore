using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Book
    {
        public int ID { get; set; }
        [Display(Name = "Book Name")]
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string BookName { get; set; }

        [Display(Name = "Author")]
        [RegularExpression(@"^[A-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "The Author should contain only Letters started with Uppercase Letter") ]
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string BookAuthor { get; set; }

        [RegularExpression(@"^\d{4}$", ErrorMessage = "Please Enter Valid Year")]
        [Display(Name = "Year Published")]
        [Required]
        public string BookYear { get; set; }

        [Display(Name = "Description")]
        [Required]
        //[StringLength(200, MinimumLength = 20, ErrorMessage ="Please write at least 20 Characters")]
        public string BookDesc { get; set; }

        
        public string ImageUrl { get; set; }

        public string BlobReference { get; set; }

        [Display(Name = "Preffered Book?")]
        public bool IsPrefferedBook { get; set; }

        [Display(Name = "Price")]
        [RegularExpression(@"^\d{0,8}(\.\d{1,2})?$", ErrorMessage = "Please Enter Valid Price")]
        [DataType(DataType.Currency)]
        public decimal BookPrice { get; set; }

        [Display(Name = "Stock")]
        public bool BookInStock { get; set; }

        [Display(Name = "Genre")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        

    }
}
