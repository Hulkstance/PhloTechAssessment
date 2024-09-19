using Phlo.TechAssessment.Api.Contracts;
using Phlo.TechAssessment.Api.Models;

namespace Phlo.TechAssessment.Api.Services;

public interface IProductProcessor
{
    ProductResponse ProcessProducts(IReadOnlyCollection<Product> products, decimal? minPrice, decimal? maxPrice, string? size, string? highlight);
}

public class ProductProcessor(IProductFilter productFilter, IProductHighlighter productHighlighter) : IProductProcessor
{
    public ProductResponse ProcessProducts(IReadOnlyCollection<Product> products, decimal? minPrice, decimal? maxPrice, string? size, string? highlight)
    {
        var filteredProducts = productFilter.FilterProducts(products, minPrice, maxPrice, size).ToList();
        var highlightedProducts = productHighlighter.HighlightProducts(filteredProducts, highlight).ToList();

        return new ProductResponse
        {
            Products = highlightedProducts,
            FilterInfo = new FilterInfo
            {
                MinPrice = filteredProducts.Count > 0 ? filteredProducts.Min(p => p.Price) : 0,
                MaxPrice = filteredProducts.Count > 0 ? filteredProducts.Max(p => p.Price) : 0,
                Sizes = filteredProducts.SelectMany(p => p.Sizes).Distinct().ToList(),
                CommonWords = GetCommonWords(filteredProducts)
            }
        };
    }

    private static List<string> GetCommonWords(IEnumerable<Product> products)
    {
        return products
            .SelectMany(p => p.Description.Split(Constants.TextProcessing.WordDelimiters, StringSplitOptions.RemoveEmptyEntries))
            .GroupBy(w => w)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .Skip(5)
            .Take(10)
            .ToList();
    }
}