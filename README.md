# LibAdminSystem
A library management system using ASP.NET Core + EF Core + LINQ + MySQL

Install the following NuGet packages:
	
	- `Microsoft.EntityFrameworkCore`
	
	- `Pomelo.EntityFrameworkCore.MySql` - `Pomelo` is the go-to EF Core provider for MySQL.
	
	- `Microsoft.EntityFrameworkCore.Tools`	
	
	- `Microsoft.EntityFrameworkCore.Design`

Add Migration & Create Database

Ensure that your MySQL server is running and the proper connection string is mentioned in `appsetting.json` as below:

```json
{
   "ConnectionStrings": {
		"LibAdminSystem": "<your MySQL connection string>"
  }
}
```

See [this](https://stackoverflow.com/questions/29866204/how-to-change-the-default-port-of-mysql-from-3306-to-3360) regarding how to change MySQL server port.


Run `dotnet tool install --global dotnet-ef` to install dotnet-ef if you haven't already.

Then this will create the MySQL database with the required tables.

```dotnet
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Run `Program.cs` to seed the database with sample data.
