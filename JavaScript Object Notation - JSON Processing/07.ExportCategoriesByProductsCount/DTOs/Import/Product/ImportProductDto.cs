namespace ProductShop.DTOs.Import.Product;

using Newtonsoft.Json;

public class ImportProductDto
{ 
    public string Name { get; set; } = null!;
     
    public decimal Price { get; set; }
     
    public int SellerId { get; set; }
     
    public int? BuyerId { get; set; }
}