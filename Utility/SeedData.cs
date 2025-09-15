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
                    new Book { Title = "1984", Author = "George Orwell", Year = 1949, Genre = "Dystopian", CopiesAvailable = 5 },
                    new Book { Title = "The Hobbit", Author = "J.R.R. Tolkien", Year = 1937, Genre = "Fantasy", CopiesAvailable = 3 }
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

            context.SaveChanges();
        }
    }

}
