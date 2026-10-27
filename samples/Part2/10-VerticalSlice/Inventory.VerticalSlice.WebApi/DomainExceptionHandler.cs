using Inventory.VerticalSlice.WebApi.Domain;
using Microsoft.AspNetCore.Diagnostics;

namespace Inventory.VerticalSlice.WebApi;

/// <summary>業務ルール違反（DomainException）を 400 エラーに変換する。全機能で共通。</summary>
public class DomainExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not DomainException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(new { error = exception.Message }, cancellationToken);
        return true;
    }
}
