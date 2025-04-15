using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Library.Pages
{
    public class LoansModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public LoansModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Loan> Loans { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }

            var userId = _userManager.GetUserId(User);

            if (User.IsInRole("Administrator"))
            {
                Loans = await _context.Loans
                    .Include(l => l.Book)
                    .ThenInclude(b => b.Genre)
                    .Include(l => l.User)
                    .OrderByDescending(l => l.LoanDate)
                    .ToListAsync();
            }
            else
            {
                Loans = await _context.Loans
                    .Where(l => l.UserId == userId)
                    .Include(l => l.Book)
                    .ThenInclude(b => b.Genre)
                    .Include(l => l.User)
                    .OrderByDescending(l => l.LoanDate)
                    .ToListAsync();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostReturnAsync(int loanId, int bookId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }

            var loan = await _context.Loans.FindAsync(loanId);

            if (loan == null)
            {
                return NotFound();
            }

            if (loan.UserId != _userManager.GetUserId(User) && !User.IsInRole("Administrator"))
            {
                return Forbid();
            }

            loan.ReturnDate = DateTime.Now;

            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                book.IsAvailable = true;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}