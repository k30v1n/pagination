using Bogus;
using Microsoft.EntityFrameworkCore;
using pagination.Data;

namespace pagination.Seeds;

public class SeedDataGeneration
{
    const int SEED = 1;

    public void GenerateSeedData(DataDbContext db)
    {
        var firstFaker = new Faker<FirstExampleData>().Rules((faker, data) =>
        {
            var person = new Bogus.Person(seed: SEED);
            data.FirstName = person.FirstName;
            data.LastName = person.LastName;
            data.DateOfBirth = person.DateOfBirth;
            data.GrossAmount = person.Random.Decimal(max: 30000);
        });

        FirstExampleData test = firstFaker.Generate();
    }
}
