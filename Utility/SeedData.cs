using LibAdminSystem.Context;
using LibAdminSystem.Models;

namespace LibAdminSystem.Utility
{
    public static class SeedData
    {
        public static void Initialize(LibraryContext context)
        {
            // Ensure DB is created
            context.Database.EnsureCreated();

            // Seed Books
            if (!context.Books.Any())
            {
                var books = new[]
                {
                    new Book { BookTitle = "1984", Author = "George Orwell", Year = 1949, Genre = "Dystopian", CopiesAvailable = 5 },
                    new Book { BookTitle = "The Hobbit", Author = "J.R.R. Tolkien", Year = 1937, Genre = "Fantasy", CopiesAvailable = 3 }
                };

                context.Books.AddRange(books);
            }

            // Seed Members
            if (!context.Members.Any())
            {
                var members = new[]
                {
                    new Member { Name = "Alice", Email = "alice@mail.com", JoinDate = DateTime.Now.AddYears(-1) },
                    new Member { Name = "Bob", Email = "bob@mail.com", JoinDate = DateTime.Now.AddMonths(-6) }
                };

                context.Members.AddRange(members);
            }

            if (!context.Loans.Any())
            {
                var loans = new[]
                {
                    new Loan { BookId = 1, MemberId = 1, LoanDate = DateTime.Now.AddDays(-10), ReturnDate = null },
                    new Loan { BookId = 2, MemberId = 1, LoanDate = DateTime.Now.AddDays(-12), ReturnDate = null },
                    new Loan { BookId = 2, MemberId = 2, LoanDate = DateTime.Now.AddDays(-20), ReturnDate = DateTime.Now.AddDays(-5) }
                };
                context.Loans.AddRange(loans);
            }

            context.SaveChanges();
        }
    }

}
