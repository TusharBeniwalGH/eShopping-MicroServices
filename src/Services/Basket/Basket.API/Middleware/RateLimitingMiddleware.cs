using Grpc.Core;
using System.Collections.Concurrent;

namespace Basket.API.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly ConcurrentDictionary<string, RequestLog> _requestLogs = new();

        public RateLimitingMiddleware(RequestDelegate next) { 
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            string ipAddress = Convert.ToString(httpContext.Connection.RemoteIpAddress);

            var now = DateTime.UtcNow;

            var requestLog = _requestLogs.GetOrAdd(ipAddress, new RequestLog());

            lock (requestLog)
            {
                requestLog.Requests.Enqueue(now);

                while (requestLog.Requests.Count > 0 && (now - requestLog.Requests.Peek()).TotalSeconds > 60)
                {
                    requestLog.Requests.Dequeue();
                }

                if (requestLog.Requests.Count > 10)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    httpContext.Response.WriteAsync("Rate Limit Exceeded");
                    return;
                }
            }
            await _next(httpContext);
        }
    }

    public class RequestLog
    {
        public Queue<DateTime> Requests { get; } = new();
    }
}
