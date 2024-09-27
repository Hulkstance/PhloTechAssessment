using Microsoft.AspNetCore.Mvc;
using Phlo.TechAssessment.Api.Auth;
using Phlo.TechAssessment.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IProductFilter, ProductFilter>();
builder.Services.AddSingleton<IProductHighlighter, ProductHighlighter>();
builder.Services.AddSingleton<IProductProcessor, ProductProcessor>();

builder.Services.AddHttpClient<ProductService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<ApiKeyOperationFilter>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/filter", async (
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] string? size,
        [FromQuery] string? highlight,
        [FromServices] IProductService productService,
        [FromServices] IProductProcessor productProcessor,
        CancellationToken cancellationToken) =>
    {
        var productsResult = await productService.GetProductsAsync(cancellationToken);
    
        return productsResult.Match(
            products => 
            {
                var result = productProcessor.ProcessProducts(products, minPrice, maxPrice, size, highlight);
                return Results.Ok(result);
            },
            exception => 
            {
                return Results.Problem("An error occurred while retrieving products", statusCode: 500);
            }
        );
    })
    .WithName("FilterProducts")
    .WithOpenApi()
    .AddEndpointFilter<ApiKeyEndpointFilter>();

app.Run();
