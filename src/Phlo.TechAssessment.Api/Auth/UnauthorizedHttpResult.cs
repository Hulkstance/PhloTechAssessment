namespace Phlo.TechAssessment.Api.Auth;

/// <summary>
/// Represents an <see cref="IResult"/> that when executed will
/// produce an HTTP response with the Unauthorized (401) status code and a custom message.
/// </summary>
/// <param name="message">The error message to be returned in the response body.</param>
public sealed class UnauthorizedHttpResult(string message) : IResult, IStatusCodeHttpResult
{
    /// <summary>
    /// Gets the HTTP status code: <see cref="StatusCodes.Status401Unauthorized"/>
    /// </summary>
    public int StatusCode => StatusCodes.Status401Unauthorized;

    int? IStatusCodeHttpResult.StatusCode => StatusCode;

    /// <inheritdoc />
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        
        httpContext.Response.StatusCode = StatusCode;
        httpContext.Response.ContentType = "text/plain";

        await httpContext.Response.WriteAsync(message);
    }
}
