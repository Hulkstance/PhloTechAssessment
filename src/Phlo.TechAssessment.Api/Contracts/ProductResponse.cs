using Phlo.TechAssessment.Api.Models;

namespace Phlo.TechAssessment.Api.Contracts;

public class ProductResponse
{
    public List<Product> Products { get; set; }
    public FilterInfo FilterInfo { get; set; }
}

public class FilterInfo
{
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public List<string> Sizes { get; set; }
    public List<string> CommonWords { get; set; }
}