using LibAdminSystem.Context;
using LibAdminSystem.Models;
using LibAdminSystem.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

public class Program
{

    private static void SampleLINQ(LibraryContext context)
    {
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
            .Select(g => new { BookTitle = g.Key, BorrowCount = g.Count() })
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

        foreach (var book in multiBorrowed)
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

        foreach (var member in topRecentMembers)
        {
            Console.WriteLine($"{member.Name} - {member.Loans.Count} - {member.JoinDate:d}");
        }
    }

    public static void Main(string[] args)
    {
        const bool SHOULD_LINQ_BE_RUN = false;
        var builder = WebApplication.CreateBuilder(args);

        // Add Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Register DbContext via DI
        builder.Services.AddDbContext<LibraryContext>(options =>
            options.UseMySql(
                builder.Configuration.GetConnectionString("LibAdminSystem"),
                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("LibAdminSystem"))
            ));

        var app = builder.Build();

        // Enable Swagger in dev env
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(); // Generates the metadata json at /swagger/v1/swagger.json
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LibAdminSystem API v1");
                c.RoutePrefix = string.Empty; // Swagger running at root `/`
            });
        }

        // Run seeding once at startup
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
            SeedData.Initialize(context);
            if (SHOULD_LINQ_BE_RUN)
                SampleLINQ(context);
        }

        var booksApi = app.MapGroup("/api/books");

        // GET /api/books -> return all books
        booksApi.MapGet("/", async (LibraryContext db) =>
            await db.Books.ToListAsync()
        );

        // GET /api/books/{id} -> return a single book
        booksApi.MapGet("/{id:int}", async (int id, LibraryContext db) =>
            await db.Books.FindAsync(id) is Book book
                ? Results.Ok(book)
                : Results.NotFound()
        );

        var membersApi = app.MapGroup("/api/members");

        // GET /api/members/{id} -> return a single member
        membersApi.MapGet("/{id:int}", async (int id, LibraryContext db) =>
            await db.Members.FindAsync(id) is Member member
                ? Results.Ok(member)
                : Results.NotFound()
        );

        // POST /api/members -> add a new member
        membersApi.MapPost("/add", async (Member member, LibraryContext db) =>
        {
            db.Members.Add(member);
            await db.SaveChangesAsync();
            return Results.Created($"/api/members/{member.Id}", member);
        });

        var loansApi = app.MapGroup("/api/loans");

        // GET /api/loans/{id} -> return a single loan
        loansApi.MapGet("/{id:int}", async (int id, LibraryContext db) =>
            await db.Loans.FindAsync(id) is Loan loan
                ? Results.Ok(loan)
                : Results.NotFound()
        );

        // PUT /api/loan/{id} -> update an existing loan's return date
        loansApi.MapPut("/{id:int}", async (int id, Loan loan, LibraryContext db) =>
        {
            var currentLoan = await db.Loans.FindAsync(id);
            if (currentLoan == null) return Results.NotFound();

            currentLoan.ReturnDate = loan.ReturnDate;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        // DELETE /api/loan/{id} -> delete a loan with given id
        loansApi.MapDelete("{id:int}", async (int id, LibraryContext db) =>
        {
            var currentLoan = await db.Loans.FindAsync(id);
            if (currentLoan == null) return Results.NotFound();

            db.Loans.Remove(currentLoan);
            await db.SaveChangesAsync();
            return Results.NoContent();

        });


        app.Run();
    }
}

