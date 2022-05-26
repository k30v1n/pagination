namespace pagination.Data;

public record Seed(int Example, bool Applied);

public class FirstExampleData : UserBase { }
public class SecondExampleData : UserBase { }
public class ThirdExampleData : UserBase { }
public class ForthExampleData : UserBase { }

public abstract class UserBase
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal GrossAmount { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }

}
