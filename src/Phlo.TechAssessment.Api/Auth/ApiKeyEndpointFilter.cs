namespace Phlo.TechAssessment.Api.Auth;

public sealed class ApiKeyEndpointFilter(IConfiguration configuration) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
        {
            return new UnauthorizedHttpResult("API Key missing");
        }

        var apiKey = configuration.GetValue<string>(AuthConstants.ApiKeyConfigurationKey)!;
        if (!apiKey.Equals(extractedApiKey))
        {
            return new UnauthorizedHttpResult("Invalid API Key");
        }
        
        return await next(context);
    }
}
