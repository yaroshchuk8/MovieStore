# MovieStore - ASP.NET Core MVC App
## Simple implementation of Web Store, where admin can upload content and customers can buy it.
## Requirements: .NET 8 SDK, SQL Server
### Topics covered:
- MVC pattern
- EF Core
    - CRUD
    - Migrations
    - Many-to-many relationships
- Identity
    - Authentication
    - Authorization (Roles)
- N-tier architecture
- Repository and UnitOfWork patterns
- Dependency Injection

---

### Execution in terminal:
```bash
> git https://github.com/yaroshchuk8/MovieStore.git
> cd .\MovieStore\MovieStore.Web\
```
> If needed, modify connection string in ./MovieStore.Web/appsettings.json according to your SQL Server path
```bash
> dotnet ef database update
> dotnet run
```
> Terminal log: **Now listening on: http://localhost:port**

---

### In order to access Content Management menu, log in as admin using pre-defined account: { login: **"admin@gmail.com"**, password: **"Admin1234_"** }

---

### Database schema:
![Database schema](https://i.imgur.com/r7Dk156.png)
