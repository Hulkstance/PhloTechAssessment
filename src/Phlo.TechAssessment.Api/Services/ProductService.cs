using System.Text.Json;
using Phlo.TechAssessment.Api.Models;
using LanguageExt.Common;

namespace Phlo.TechAssessment.Api.Services;

public interface IProductService
{
    Task<Result<IReadOnlyCollection<Product>>> GetProductsAsync(CancellationToken cancellationToken = default);
}

public class ProductService(ILogger<ProductService> logger, HttpClient httpClient) : IProductService
{
    private const string ProductApiUrl = "https://pastebin.com/raw/JucRNpWs";
    
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    
    public async Task<Result<IReadOnlyCollection<Product>>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Fetching products from {Url}", ProductApiUrl);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, ProductApiUrl);
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                logger.LogWarning("HTTP request failed with status code {StatusCode}", httpResponseMessage.StatusCode);
                return new Result<IReadOnlyCollection<Product>>(new HttpRequestException($"HTTP request failed with status code {httpResponseMessage.StatusCode}"));
            }

            var jsonString = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            var apiResponse = JsonSerializer.Deserialize<ProductApiResponse>(jsonString, JsonOptions);

            if (apiResponse?.Products == null)
            {
                return new Result<IReadOnlyCollection<Product>>(new JsonException("Failed to deserialize products"));
            }

            logger.LogInformation("Successfully fetched {Count} products", apiResponse.Products.Count);
            return new Result<IReadOnlyCollection<Product>>(apiResponse.Products);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while fetching products");
            return new Result<IReadOnlyCollection<Product>>(ex);
        }
    }
}