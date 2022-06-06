using System.ComponentModel.DataAnnotations;

namespace pagination.Data;

public class Seed
{
    public int ExampleId { get; set; }
}

public class FirstExampleData : UserBase { }
public class SecondExampleData : UserBase { }
public class ThirdExampleData : UserBase { }
public class ForthExampleData : UserBase { }

public class UserBase
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal GrossAmount { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }

    /// <summary>
    /// Sortable FirstName column + Unique for cursor pagination purpose
    /// </summary>
    public string? Sorting_FirstName { get; set; }

}
