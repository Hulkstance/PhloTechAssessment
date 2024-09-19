namespace Phlo.TechAssessment.Api.Models;

public class ProductApiResponse
{
    public IReadOnlyCollection<Product> Products { get; set; }
    public ApiKeys ApiKeys { get; set; }
}