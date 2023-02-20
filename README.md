# [Code First DB Schema Migration - Playground](https://learn.microsoft.com/en-us/ef/core/managing-schemas/)
EF Core provides two primary ways of keeping your EF Core model and database schema in sync.

1. If you want your EF Core model to be the source of truth, use Migrations. As you make changes to your EF Core model, this approach incrementally applies the corresponding schema changes to your database so that it remains compatible with your EF Core model.

2. Use Reverse Engineering if you want your database schema to be the source of truth. This approach allows you to scaffold a DbContext and the entity type classes by reverse engineering your database schema into an EF Core model.

**This sample solution focuses on the fist approach - Migrations.**
### Migrations Overview

At a high level, migrations function in the following way:

1. When a data model change is introduced, the developer uses EF Core tools to add a corresponding migration describing the updates necessary to keep the database schema in sync. EF Core compares the current model against a snapshot of the old model to determine the differences, and generates migration source files; the files can be tracked in your project's source control like any other source file.
2. Once a new migration has been generated, it can be applied to a database in various ways. EF Core records all applied migrations in a special history table, allowing it to know which migrations have been applied and which haven't.


### Prerequisites

You'll have to install the [EF Core command-line tools](https://learn.microsoft.com/en-us/ef/core/cli/).

Declare a [DatabaseContext](./Data/DatabaseContext.cs) class.
### Add a New Migration

The migrations are kept in the source control system and are generated and committed by the developers.
If a new table is created/altered/renamed, migrations can be automatically generated.
In the sample project, you can test that feature by changing the Blog entity.
After code changes are finalized, the following command will generated a migration script.

```powershell
dotnet ef migrations add CreateTable_Blog
dotnet ef migrations list
dotnet ef migrations --help
```

Migrations are generated in Migrations solution subfolder.
* **YYYYMMDDhhmmss_MigrationName.cs** - The migration file наме is prefixed with time  – which signifies the time at which this migration was created.
With any migration, aditional Designer file is created.
This is a file which contains two methods – Up and Down.

Up method contains code which would modify database and apply the new changes
Down method contains code which would rollback the new changes from database

* **20230220142413_CreateView_ExpenseTotal** - The migrations metadata file. Contains information used by EF.
* **YYYYMMDDhhmmss_MigrationName.Designer.cs** - The migrations metadata file. Contains information used by EF.
* [**UniversityContextModelSnapshot**](./Migrations/DatabaseContextModelSnapshot.cs) - The snapshot of your current model. This file is used to determine what changed when adding the next migration.

ref: https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
ref: https://thecodeblogger.com/2021/07/24/know-everything-about-net-ef-core-migrations/

You can fine tune the table/column properties by the [entity attributes](https://learn.microsoft.com/en-us/ef/core/modeling/entity-properties?tabs=data-annotations%2Cwithout-nrt).
#### Create/Alter View
Views cannot be infered by entites, but they can fit in the migration scripts. First you need to add a new migration

```powershell
dotnet ef migrations add CreateView_ExpenseTotal
```

Then edit the [up](./Migrations/20230220142413_CreateView_ExpenseTotal.cs) method to create/alter the view. 


The OnModelCreating can be overriden to further configure the model that was discovered by convention from the entity types exposed in DbSet<TEntity> properties on your derived context. 
In DatabaseContext, edit OnModelCreating to create the View.

```csharp
            modelBuilder
               .Entity<ExpenseByTotal>()
               .ToView("ExpenseByTotalView")
               .HasKey(t => t.Id);
```

### Naming Convention
When you are creating a migration be mindful about the name you give for the migration.
It would be great if you can maintain a naming convention in your projects for migrations such as,

+ If you are adding a new table — CreateTable_Blog
+ If you are rename Name a column to BlogName — _AlterTable_RenameName_To_BlogName
+ If you are adding more than one table, the name of the feature or some other composite name can be used — CreateTable_ExpenseHistory_ExpenseItem
+ If you are adding a new column NewColumn to a table Blog — AlterTable_Blog_AddColumn_NewColumn
+ If you are adding a view - CreateView_ExpenseTotal
+ If you are alter a view - AlterView_ExpenseTotal_StaticColumn
+ If you are seeding data — SeedData_Add_ExpenseItems
+ If you are running a SQL upload script — {timestamp}_uploadScript
#### Seed Data in Entity Framework Core
In most of our projects, we want to have some initial data in the created database. So as soon as we execute our migration files to create and configure the database, we want to populate it with some initial data. This action is called Data Seeding.


We can create the code for the seeding action in the OnModelCreating method by using the ModelBuilder in [DatabaseContext](./Data/DatabaseContext.cs).
```csharp
            modelBuilder.Entity<ExpenseItem>().HasData(
                new ExpenseItem() { Id = 1, Name = "Ferrari", Category = "Big Expense" },
                new ExpenseItem() { Id = 2, Name = "Cheese", Category = "Small Expense" },
                new ExpenseItem() { Id = 3, Name = "TV", Category = "Mid Expense" }
                );
```

After running the add migrtion command we will [have a new migration cs](./Migrations/20230220144056_SeedData_Add_ExpenseItems.cs) file with the new inserts in the up method(and corresponging delete in the down method).
For ex:
```csharp
            migrationBuilder.InsertData(
                table: "ExpenseItems",
                columns: new[] { "Id", "Category", "Name" },
                values: new object[,]
                {
                    { 1, "Big Expense", "Ferrari" },
                    { 2, "Small Expense", "Cheese" },
                    { 3, "Mid Expense", "TV" }
                });
 ```         
Ref:https://rehansaeed.com/migrating-to-entity-framework-core-seed-data/

### Migrate TEST/DEV/INTEGRATION/STAGE database
After migrations are generated, they can be incrementally executed, locally first and by the CI/CD on the local environments.
```powershell
dotnet ef database update
dotnet ef database --help
```

### Migrate PROD database
The recommended way to deploy migrations to a production database is by generating SQL scripts. The advantages of this strategy include the following:

SQL scripts can be reviewed for accuracy; this is important since applying schema changes to production databases is a potentially dangerous operation that could involve data loss.
In some cases, the scripts can be tuned to fit the specific needs of a production database.
SQL scripts can be used in conjunction with a deployment technology, and can even be generated as part of your CI process.
SQL scripts can be provided to a DBA, and can be managed and archived separately.

```powershell
dotnet ef migrations script  --idempotent
dotnet ef migrations script  --idempotent CreateView_ExpenseTotal
dotnet ef migrations script  --idempotent CreateView_ExpenseTotal AlterView_ExpenseTotal_StaticColumn -o deployToIntegration.sql
```

**Idempotent scripts** will internally check which migrations have already been applied and apply only the missing ones.
For example the check if not exists syntax below.

```sql
BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230220142747_AlterView_ExpenseTotal_StaticColumn')
BEGIN
    CREATE OR ALTER VIEW ExpenseByTotalView AS
                   SELECT p.Id,
                          p.Name,
                          p.Category,
                          sum(h.Amount) AS TotalAmount,
                          'a new column' as StaticColumn
                     FROM ExpenseItems p
                   JOIN ExpenseHistory h ON p.Id = h.ExpenseItemId
                   GROUP BY p.Id, p.Name, p.Category
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230220142747_AlterView_ExpenseTotal_StaticColumn')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230220142747_AlterView_ExpenseTotal_StaticColumn', N'7.0.3');
END;
GO

COMMIT;
GO

```
Ref: https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli

### Migration Reviews
The migration scripts can be reviewed in the pull request. 

### Using a Separate Migrations Project

This approach may have several pros as DB is migrated only if a change to the migration scripts is done. 
Also, it keeps the application code base clean from migration details.

https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/projects?tabs=dotnet-core-cli

### Reset migration script
If Migrations scripts number increases too much, the migration starting point can be reset.
Ref: https://weblog.west-wind.com/posts/2016/jan/13/resetting-entity-framework-migrations-to-a-clean-slate