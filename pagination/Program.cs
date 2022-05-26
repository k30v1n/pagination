using Microsoft.EntityFrameworkCore;
using pagination;
using pagination.Data;
using pagination.Seeds;

Console.WriteLine("Hello, World!");

var db = new DataDbContext();

Console.WriteLine("Migrating DB...");
db.Database.Migrate();
Console.WriteLine("DB migrated");


{
    var seedGeneration = new SeedDataGeneration();
    seedGeneration.GenerateSeedData(db);
}

Console.WriteLine("nice");
