# LibAdminSystem
A library management system using ASP.NET Core + EF Core + LINQ + MySQL

Install the following NuGet packages:
	- `Microsoft.EntityFrameworkCore`
	- `Pomelo.EntityFrameworkCore.MySql` - `Pomelo` is the go-to EF Core provider for MySQL.
	- `Microsoft.EntityFrameworkCore.Tools`	
	- `Microsoft.EntityFrameworkCore.Design`

Add Migration & Create Database

Run `dotnet tool install --global dotnet-ef` to install dotnet-ef if you haven't already.

Then this will create the MySQL database with the required tables.

```dotnet
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Run `Program.cs` to seed the database with sample data.
