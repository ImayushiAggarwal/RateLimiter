Distributed Rate Limiter & High-Throughput Batch Processing

A production-oriented .NET rate-limiting playground that evolves a simple custom Token Bucket into a configurable, testable, observable, and distributed throttling platform.

The project combines two complementary approaches:

1. Custom algorithms** to understand and implement rate-limiting mechanics from first principles.
2. System.Threading.RateLimiting to demonstrate idiomatic .NET primitives such as token buckets, queued permits, cancellation, and concurrency controls.

The end goal is to answer a practical distributed-systems question:

How do we keep throughput controlled and behavior correct when traffic is concurrent, queued, cancelled, and horizontally scaled across multiple API instances?

 What this project demonstrates

- ASP.NET Core middleware and dependency injection
- Custom Token Bucket implementation
- .NET `System.Threading.RateLimiting` integration
- Token Bucket, Fixed Window, Sliding Window and Concurrency limiting
- Configurable policies by endpoint/client/API key/user/tenant
- `IRateLimiter`, `IRateLimitStore`, and key-resolution abstractions
- In-memory and Redis-backed distributed state
- Atomic Redis operations and Lua scripting
- Async programming with `async/await`, `ValueTask`, and `CancellationToken`
- `Parallel.ForEachAsync` for bounded concurrent processing
- `Channel<T>` for producer/consumer pipelines and backpressure
- `BackgroundService` for long-running workers
- Batch processing with queue limits and permit acquisition
- 429 responses with `Retry-After` and RateLimit headers
- ProblemDetails responses
- Structured logging and correlation
- OpenTelemetry metrics, traces and logs
- Health checks and graceful shutdown
- Unit, integration, concurrency and distributed tests
- BenchmarkDotNet and load testing with k6/NBomito
- Docker Compose for API + Redis + observability dependencies
- GitHub Actions CI/CD
- ADRs and architecture documentation

## Architecture


                         ┌─────────────────────┐
                         │      Clients        │
                         └──────────┬──────────┘
                                    │
                                    ▼
                         ┌─────────────────────┐
                         │   ASP.NET Core API  │
                         │ Middleware/Endpoints│
                         └──────────┬──────────┘
                                    │
                         ┌──────────▼──────────┐
                         │ Rate Limit Key      │
                         │ Resolver            │
                         └──────────┬──────────┘
                                    │
                         ┌──────────▼──────────┐
                         │ Policy Provider     │
                         │ endpoint/client     │
                         └──────────┬──────────┘
                                    │
                   ┌────────────────▼────────────────┐
                   │        Rate Limiter Engine      │
                   │ Token | Fixed | Sliding | Concurrency
                   └────────────────┬────────────────┘
                                    │
                         ┌──────────▼──────────┐
                         │   Rate Limit Store  │
                         │ Memory / Redis      │
                         └──────────┬──────────┘
                                    │
                       ┌────────────▼────────────┐
                       │ Batch / Job Processing  │
                       │ Parallel.ForEachAsync   │
                       │ Channel<T> + Workers    │
                       └────────────┬────────────┘
                                    │
                                    ▼
                            ┌───────────────┐
                            │ Downstream API│
                            └───────────────┘

        Observability: OpenTelemetry → Metrics / Traces / Logs
