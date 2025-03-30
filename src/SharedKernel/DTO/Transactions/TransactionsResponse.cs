namespace SharedKernel.DTO.Transactions;

public sealed class TransactionsResponse
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; }
    public string Type { get; set; }
}
