using Phlo.TechAssessment.Api.Models;

namespace Phlo.TechAssessment.Api.Services;

public interface IProductFilter
{
    IEnumerable<Product> FilterProducts(IEnumerable<Product> products, decimal? minPrice, decimal? maxPrice, string? size);
}

public class ProductFilter : IProductFilter
{
    public IEnumerable<Product> FilterProducts(IEnumerable<Product> products, decimal? minPrice, decimal? maxPrice, string? size)
    {
        var filteredProducts = products;

        if (minPrice.HasValue)
        {
            filteredProducts = filteredProducts.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            filteredProducts = filteredProducts.Where(p => p.Price <= maxPrice.Value);
        }

        if (!string.IsNullOrEmpty(size))
        {
            var sizes = size.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            filteredProducts = filteredProducts.Where(p => sizes.All(s => p.Sizes.Contains(s, StringComparer.OrdinalIgnoreCase)));
        }

        return filteredProducts;
    }
}
