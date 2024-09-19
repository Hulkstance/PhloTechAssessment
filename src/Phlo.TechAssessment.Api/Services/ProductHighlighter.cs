using Phlo.TechAssessment.Api.Models;

namespace Phlo.TechAssessment.Api.Services;

public interface IProductHighlighter
{
    IEnumerable<Product> HighlightProducts(IEnumerable<Product> products, string? highlightWords);
}

public class ProductHighlighter : IProductHighlighter
{
    public IEnumerable<Product> HighlightProducts(IEnumerable<Product> products, string? highlightWords)
    {
        if (string.IsNullOrEmpty(highlightWords))
        {
            return products;
        }

        var words = highlightWords.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return products.Select(product => new Product
        {
            Title = product.Title,
            Price = product.Price,
            Sizes = product.Sizes,
            Description = HighlightDescription(product.Description, words)
        });
    }

    private static string HighlightDescription(string description, IEnumerable<string> words)
    {
        return words.Aggregate(description, (current, word) => 
            current.Replace(word, $"<em>{word}</em>", StringComparison.OrdinalIgnoreCase));
    }
}