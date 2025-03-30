namespace SharedKernel.DTO.Accounts;

public class AccountCreateRequest
{
    public string AccountName { get; set; }
    public string AccountType { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; }
}
