using Inventory.Layered.Business;
using Microsoft.AspNetCore.Diagnostics;

namespace Inventory.Layered.Presentation;

/// <summary>業務ロジック層の例外を HTTP のエラー応答に変換する。</summary>
public class BusinessExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var statusCode = exception switch
        {
            ProductNotFoundException => StatusCodes.Status404NotFound,
            BusinessException => StatusCodes.Status400BadRequest,
            _ => (int?)null,
        };

        if (statusCode is null)
        {
            return false; // 想定外の例外は既定の処理（500エラー）に任せる
        }

        httpContext.Response.StatusCode = statusCode.Value;
        await httpContext.Response.WriteAsJsonAsync(new { error = exception.Message }, cancellationToken);
        return true;
    }
}
