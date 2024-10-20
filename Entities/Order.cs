using System.Text.Json.Serialization;

public class Order
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime OrderDate { get; set; }
    public int SupplierId { get; set; }
    public string Status { get; set; } = string.Empty;
    [JsonIgnore]
    public Product? Product { get; set; }
    [JsonIgnore]
    public Supplier? Supplier { get; set; }
}