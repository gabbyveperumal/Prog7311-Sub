namespace GLMS.API.Data;

public class Contract
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Currency { get; set; } = "ZAR";
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}