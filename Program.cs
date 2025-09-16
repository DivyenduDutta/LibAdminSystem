using LibAdminSystem.Context;
using LibAdminSystem.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<LibraryContext>(options =>
            options.UseMySql(
                context.Configuration.GetConnectionString("LibAdminSystem"),
                ServerVersion.AutoDetect(context.Configuration.GetConnectionString("LibAdminSystem"))
            ));
    })
    .Build();

using var scope = host.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();

SeedData.Initialize(context);

// LINQ queries

// 1. Get all Books

var books = context.Books.ToList();
Console.WriteLine("All Books in Library: ");
foreach (var book in books)
{
    Console.WriteLine($"{book.BookTitle} by {book.Author}");
}

// 2. Find overdue books (loaned > 7 days ago and not returned)

var overdue = context.Loans
    .Where(l => l.ReturnDate == null && l.LoanDate < DateTime.Now.AddDays(-7))
    .Include(l => l.Book)
    .Include(l => l.Member)
    .ToList();

Console.WriteLine("\nOverdue Books: ");
foreach (var loan in overdue)
{
    Console.WriteLine($"{loan.Book.BookTitle} loaned to {loan.Member.Name} on {loan.LoanDate:d}");
}

// 3. Top 3 most borrowed books

var topBooks = context.Loans
    .GroupBy(l => l.Book.BookTitle)
    .Select(g => new { BookTitle = g.Key, BorrowCount = g.Count()})
    .OrderByDescending(b => b.BorrowCount)
    .Take(3)
    .ToList();

Console.WriteLine("\nTop 3 Most Borrowed Books: ");

foreach (var book in topBooks) 
{
    Console.WriteLine($"{book.BookTitle} was borrowed {book.BorrowCount} times");
}

// 4. All books borrowed by more than one member

var multiBorrowed = context.Loans
    .GroupBy(l => l.Book.BookTitle)
    .Select(g => new { BookTitle = g.Key, BorrowCount = g.Select(l => l.MemberId).Distinct().Count() })
    .Where(x => x.BorrowCount > 1)
    .ToList();

Console.WriteLine("\nBooks borrowed by more than one member");

foreach(var book in multiBorrowed) 
{
    Console.WriteLine($"{book.BookTitle} was borrowed {book.BorrowCount} times");
}

// 5. Top 3 most recently joined members who currently have at least 1 active loan (not returned)

var topRecentMembers = context.Members
    .Where(m => m.Loans.Any(l => l.ReturnDate == null))
    .OrderByDescending(m => m.JoinDate)
    .Take(3)
    .ToList();

Console.WriteLine("\nTop 3 most recently joined members who currently have at least 1 active loan (not returned)");

foreach(var member in topRecentMembers)
{
    Console.WriteLine($"{member.Name} - {member.Loans.Count} - {member.JoinDate:d}");
}


/*var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run(); */
