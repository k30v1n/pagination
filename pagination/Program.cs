using System.Diagnostics;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;
using pagination.Data;
using pagination.Seeds;

Console.WriteLine("Welcome to pagination benchmarking!");
var sw = Stopwatch.StartNew();

using (var db = new DataDbContext())
{ 
    Console.WriteLine("Migrating DB...");
    db.Database.Migrate();
    Console.WriteLine("DB migrated");

    var seedGeneration = new SeedDataGeneration();
    seedGeneration.GenerateSeedData();
}

Console.WriteLine("Running benchmarks...");

var benchmark = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly);
#if DEBUG
//var summary = benchmark.Run(args: new string[] { "--filter", "*" }, new DebugInProcessConfig());
var summary = benchmark.Run(args: args, new DebugInProcessConfig());
#else
var summary = benchmark.Run(args, DefaultConfig.Instance);
#endif

Console.WriteLine($"Finished - after {sw.Elapsed.TotalMinutes} minutes");
