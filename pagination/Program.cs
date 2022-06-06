using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;
using pagination;
using pagination.Data;
using pagination.Seeds;

Console.WriteLine("Welcome to pagination benchmarking!");

using (var db = new DataDbContext())
{ 
    Console.WriteLine("Migrating DB...");
    db.Database.Migrate();
    Console.WriteLine("DB migrated");

    var seedGeneration = new SeedDataGeneration();
    seedGeneration.GenerateSeedData(db);
}

Console.WriteLine("Running benchmarks...");

var benchmark = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly);
#if DEBUG
var summary = benchmark.Run(args: new string[] { "--filter", "*" }, new DebugInProcessConfig());
#else
var summary = benchmark.Run(args, DefaultConfig.Instance);
#endif

Console.WriteLine("Finished");
