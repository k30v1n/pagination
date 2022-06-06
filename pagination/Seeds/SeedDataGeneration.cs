using System.Diagnostics;
using System.Text;
using Bogus;
using Microsoft.EntityFrameworkCore;
using pagination.Data;

namespace pagination.Seeds;

public class SeedDataGeneration
{
    record SeedSetup(int ExampleId, int RecordsCount, string TableName);

    public void GenerateSeedData()
    {
        var seedSetups = new SeedSetup[]
        {
            new SeedSetup(1, 1000, "1_FirstExample"),
            new SeedSetup(2, 100000, "2_SecondExample"),
            new SeedSetup(3, 1000000, "3_ThirdExample"),
            new SeedSetup(4, 5000000, "4_ForthExample")
        };

        var faker = CreateExampleFaker();

        seedSetups.AsParallel().ForAll(setup =>
        {
            using var seedDbContext = new DataDbContext();

            if (!seedDbContext.Seeds.Any(x => x.ExampleId == setup.ExampleId))
            {
                var stopwatch = Stopwatch.StartNew();
                Console.WriteLine($"Example {setup.ExampleId} - Generating {setup.RecordsCount} records...");

                var generatedData = faker.GenerateLazy(setup.RecordsCount);

                foreach (var batchData in BatchData(generatedData))
                {
                    var bulkInsert = new StringBuilder($"INSERT INTO `{setup.TableName}`(`FirstName`,`LastName`,`GrossAmount`,`DateOfBirth`, `Sorting_FirstName`) VALUES");
                    foreach (var dataItem in batchData)
                    {
                        bulkInsert.Append($"(\"{dataItem.FirstName}\",\"{dataItem.LastName}\",{dataItem.GrossAmount},\"{dataItem.DateOfBirth.ToString("yyyy-MM-dd")}\",\"{dataItem.Sorting_FirstName}\"),");
                    }

                    bulkInsert.Remove(bulkInsert.Length - 1, 1);
                    bulkInsert.Append(";");

                    seedDbContext.Database.ExecuteSqlRaw(bulkInsert.ToString());
                }


                seedDbContext.Seeds.Add(new Seed { ExampleId = setup.ExampleId });
                seedDbContext.SaveChanges();

                Console.WriteLine($"Example {setup.ExampleId} - records generated in database {setup.RecordsCount} after {stopwatch.ElapsedMilliseconds}ms");
            }
        });
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

    private static Faker<UserBase> CreateExampleFaker() => new Faker<UserBase>().Rules((faker, data) =>
    {
        var person = new Bogus.Person(seed: faker.UniqueIndex);
        data.FirstName = person.FirstName;
        data.LastName = person.LastName;
        data.DateOfBirth = person.DateOfBirth;
        data.GrossAmount = person.Random.Decimal(max: 30000);

        // Generating sortable+unique column for cursor pagination+sorting
        // mapping to EF just for example purposes
        data.Sorting_FirstName = person.FirstName + Guid.NewGuid().ToString();
    });
}
