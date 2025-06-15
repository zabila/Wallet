namespace Domain.Transactions;

public sealed class Location
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public static implicit operator Location(SharedKernel.DTO.Transactions.Location v) => new()
    {
        Latitude = v.Latitude,
        Longitude = v.Longitude
    };
}
