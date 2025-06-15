using Bogus;

namespace TestUtilities.Extensions;

public static class TestDataExtensions
{
    private static readonly Faker Faker = new();

    public static string GenerateRandomString(int length = 10)
    {
        if (length <= 0)
        {
            return string.Empty;
        }
        return Faker.Random.String2(length);
    }

    public static string GenerateRandomEmail()
    {
        return Faker.Internet.Email();
    }

    public static string GenerateRandomName()
    {
        return Faker.Name.FullName();
    }

    public static string GenerateRandomUsername()
    {
        return Faker.Internet.UserName();
    }

    public static decimal GenerateRandomDecimal(decimal min = 0, decimal max = 1000)
    {
        return Faker.Random.Decimal(min, max);
    }

    public static int GenerateRandomInt(int min = 1, int max = 1000000)
    {
        return Faker.Random.Int(min, max);
    }

    public static long GenerateRandomLong(long min = 1, long max = 1000000000)
    {
        return Faker.Random.Long(min, max);
    }

    public static DateTime GenerateRandomDateTime()
    {
        return Faker.Date.Recent();
    }

    public static DateTime GenerateRandomFutureDateTime()
    {
        return Faker.Date.Future();
    }

    public static DateTime GenerateRandomPastDateTime()
    {
        return Faker.Date.Past();
    }

    public static T GenerateRandomEnum<T>() where T : struct, Enum
    {
        return Faker.PickRandom<T>();
    }
}