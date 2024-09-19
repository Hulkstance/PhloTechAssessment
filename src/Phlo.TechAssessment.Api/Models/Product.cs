namespace Phlo.TechAssessment.Api.Models;

public class Product
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string[] Sizes { get; set; }
    public string Description { get; set; }
}
