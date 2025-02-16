namespace SharedKernel.DTO.Transactions;
public class TransactionsRequest
{
    public decimal Amount { get; set; }
    public string Category { get; set; }
    public string Type { get; set; }
    public Location Location { get; set; }
    public string Attachment { get; set; }
}
