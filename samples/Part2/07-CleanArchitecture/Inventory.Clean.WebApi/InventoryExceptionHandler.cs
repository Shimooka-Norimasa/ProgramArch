using Inventory.Clean.Application;
using Inventory.Clean.Domain;
using Microsoft.AspNetCore.Diagnostics;

namespace Inventory.Clean.WebApi;

/// <summary>内側の層の例外を HTTP のエラー応答に変換する。HTTP の知識は外側だけが持つ。</summary>
public class InventoryExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var statusCode = exception switch
        {
            ProductNotFoundException => StatusCodes.Status404NotFound,
            DomainException => StatusCodes.Status400BadRequest,
            _ => (int?)null,
        };

        if (statusCode is null)
        {
            return false;
        }

        httpContext.Response.StatusCode = statusCode.Value;
        await httpContext.Response.WriteAsJsonAsync(new { error = exception.Message }, cancellationToken);
        return true;
    }
}
