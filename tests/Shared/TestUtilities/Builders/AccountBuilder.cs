using Domain.Accounts;

namespace TestUtilities.Builders;

public class AccountBuilder
{
    private Guid _id;
    private string _accountName = "Test Account";
    private DateTime _createdAt = DateTime.UtcNow;
    private DateTime _updatedAt = DateTime.UtcNow;
    private string _accountType = "Checking";
    private decimal _balance = 100.00m;
    private string _currency = "USD";

    public static AccountBuilder Create() => new();

    public AccountBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public AccountBuilder WithAccountName(string accountName)
    {
        _accountName = accountName;
        return this;
    }

    public AccountBuilder WithAccountType(string accountType)
    {
        _accountType = accountType;
        return this;
    }

    public AccountBuilder WithBalance(decimal balance)
    {
        _balance = balance;
        return this;
    }

    public AccountBuilder WithCurrency(string currency)
    {
        _currency = currency;
        return this;
    }

    public AccountBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public AccountBuilder WithUpdatedAt(DateTime updatedAt)
    {
        _updatedAt = updatedAt;
        return this;
    }

    public Account Build()
    {
        return new Account
        {
            Id = _id == Guid.Empty ? Guid.NewGuid() : _id,
            AccountName = _accountName,
            CreatedAt = _createdAt,
            UpdatedAt = _updatedAt,
            AccountType = _accountType,
            Balance = _balance,
            Currency = _currency
        };
    }
}