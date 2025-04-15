using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Library.Models
{
    public class Loan
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Display(Name = "Loan Date")]
        [DataType(DataType.Date)]
        public DateTime LoanDate { get; set; } = DateTime.Now;

        [Display(Name = "Return Date")]
        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        public virtual Book Book { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}