using LibAdminSystem.Context;
using LibAdminSystem.Models;

using var context = new LibraryContext();

// Ensure DB created
context.Database.EnsureCreated();

// Seed data if empty
if (!context.Books.Any())
{
    var book1 = new Book { Title = "1984", Author = "George Orwell", Year = 1949, Genre = "Dystopian", CopiesAvailable = 5 };
    var book2 = new Book { Title = "The Hobbit", Author = "J.R.R. Tolkien", Year = 1937, Genre = "Fantasy", CopiesAvailable = 3 };

    var member1 = new Member { Name = "Alice", Email = "alice@mail.com", JoinDate = DateTime.Now.AddYears(-1) };
    var member2 = new Member { Name = "Bob", Email = "bob@mail.com", JoinDate = DateTime.Now.AddMonths(-6) };

    context.AddRange(book1, book2, member1, member2);
    context.SaveChanges();
}

/*var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run(); */
