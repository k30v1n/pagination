using System.Diagnostics;
using System.Text;
using Bogus;
using Microsoft.EntityFrameworkCore;
using pagination.Data;

namespace pagination.Seeds;

public class SeedDataGeneration
{
    const int SEED = 1;
    record SeedSetup(int ExampleId, int RecordsCount, string TableName);

    public void GenerateSeedData(DataDbContext db)
    {
        var seeds = db.Seeds.ToList();

        var seedSetups = new SeedSetup[]
        {
            new SeedSetup(1, 1000, "1_FirstExample"),
            new SeedSetup(2, 10000, "2_SecondExample"),
            new SeedSetup(3, 100000, "3_ThirdExample"),
            new SeedSetup(4, 1000000, "4_ForthExample")
        };

        var faker = GetExampleFaker();

        foreach (var seedSetup in seedSetups)
        {
            if (!seeds.Any(x => x.ExampleId == seedSetup.ExampleId))
            {
                var stopwatch = Stopwatch.StartNew();
                Console.WriteLine($"Example {seedSetup.ExampleId} - Generating {seedSetup.RecordsCount} records...");

                var data = faker.GenerateLazy(seedSetup.RecordsCount);

                foreach (var batchData in BatchData(data))
                {
                    var bulkInsert = new StringBuilder($"INSERT INTO `{seedSetup.TableName}`(`FirstName`,`LastName`,`GrossAmount`,`DateOfBirth`) VALUES");
                    foreach (var batch in batchData)
                    {
                        bulkInsert.Append($"(\"{batch.FirstName}\",\"{batch.LastName}\",{batch.GrossAmount},\"{batch.DateOfBirth.ToString("yyyy-MM-dd")}\"),");
                    }
                    
                    bulkInsert.Remove(bulkInsert.Length - 1, 1);
                    bulkInsert.Append(";");

                    db.Database.ExecuteSqlRaw(bulkInsert.ToString());
                }
                

                db.Seeds.Add(new Seed { ExampleId = seedSetup.ExampleId });

                Console.WriteLine($"Example {seedSetup.ExampleId} - Applying db changes...");
                db.SaveChanges();

                Console.WriteLine($"Example {seedSetup.ExampleId} - records generated in database {seedSetup.RecordsCount} after {stopwatch.ElapsedMilliseconds}ms");
            }
        }
    }

    private static IEnumerable<IEnumerable<UserBase>> BatchData(IEnumerable<UserBase> data)
    {
        var batchSize = 10000;
        var total = 0;

        while (true)
        {
            var result = data.Take(total..(total + batchSize));

            if (!result.Any()) break;

            yield return result;

            total += batchSize;
        }
    }

    private static Faker<UserBase> GetExampleFaker() =>
        new Faker<UserBase>().Rules((faker, data) =>
        {
            var person = new Bogus.Person(seed: faker.UniqueIndex);
            data.FirstName = person.FirstName;
            data.LastName = person.LastName;
            data.DateOfBirth = person.DateOfBirth;
            data.GrossAmount = person.Random.Decimal(max: 30000);
        });
}
