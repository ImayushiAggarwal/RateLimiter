using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;

public class TokenBucketRateLimiterMiddleware
{
    private readonly RequestDelegate _next;

    // ✔️ Thread-safe dictionary
    private static readonly ConcurrentDictionary<string, TokenBucket> Buckets 
        = new();

    // ⚙️ Config
    private const int BucketCapacity = 10;      // max tokens
    private const double RefillRate = 1;         // tokens added per second

    public TokenBucketRateLimiterMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // 📌 Identify user (IP/or API key)
        var key = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var bucket = Buckets.GetOrAdd(key, _ => new TokenBucket
        {
            Tokens = BucketCapacity,
            LastRefill = DateTime.UtcNow
        });

        lock (bucket) // 🛡 thread safety
        {
            var now = DateTime.UtcNow;

            // ⏳ Time since last refill
            var elapsed = (now - bucket.LastRefill).TotalSeconds;

            // 💧 Refill tokens based on time passed
            var refill = elapsed * RefillRate;
            bucket.Tokens = Math.Min(BucketCapacity, bucket.Tokens + refill);

            bucket.LastRefill = now;

            // ❌ No tokens = Reject request
            if (bucket.Tokens < 1)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.Response.Headers.RetryAfter = "5"; // seconds (hint)
                context.Response.ContentType = "text/plain";
                return;
            }

            // ✔️ Consume token
            bucket.Tokens -= 1;
        }

        await _next(context);
    }
}
