using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; } = true;

        [ForeignKey("GenreId")]
        public virtual Genre Genre { get; set; }

        public virtual ICollection<Loan> Loans { get; set; }
    }
}