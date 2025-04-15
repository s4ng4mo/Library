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
    public class BooksModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BooksModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Book> Books { get; set; }
        public IList<Genre> Genres { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync()
        {
            Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();

            var books = _context.Books.Include(b => b.Genre).AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                books = books.Where(b => b.Title.Contains(SearchString) ||
                                        b.Author.Contains(SearchString));
            }

            Books = await books.ToListAsync();
        }

        public async Task<IActionResult> OnPostCreateAsync(string title, string author, int genreId)
        {
            if (!User.IsInRole("Administrator"))
            {
                return Forbid();
            }

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author) || genreId == 0)
            {
                await OnGetAsync();
                return Page();
            }

            var book = new Book
            {
                Title = title,
                Author = author,
                GenreId = genreId,
                IsAvailable = true
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(int id, string title, string author, int genreId)
        {
            if (!User.IsInRole("Administrator"))
            {
                return Forbid();
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            book.Title = title;
            book.Author = author;
            book.GenreId = genreId;

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!User.IsInRole("Administrator"))
            {
                return Forbid();
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostBorrowAsync(int bookId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }

            var book = await _context.Books.FindAsync(bookId);

            if (book == null)
            {
                return NotFound();
            }

            if (!book.IsAvailable)
            {
                return RedirectToPage();
            }

            var userId = _userManager.GetUserId(User);

            var loan = new Loan
            {
                BookId = bookId,
                UserId = userId,
                LoanDate = DateTime.Now,
                ReturnDate = null
            };

            book.IsAvailable = false;

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}