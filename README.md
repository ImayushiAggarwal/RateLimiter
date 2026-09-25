# 🚦 Distributed Rate Limiter

A production-oriented ASP.NET Core / .NET 10 rate limiting system that demonstrates how to design, implement, test, and scale rate limiting from a simple in-memory Token Bucket to a **distributed, configurable, observable, and highly concurrent system**.

The project combines a **custom rate limiter implementation** with .NET's built-in `System.Threading.RateLimiting` APIs and explores batch processing, concurrency control, backpressure, Redis-based distributed state, observability, and performance testing.

> **Core engineering problem:** How do we control request throughput correctly when thousands of requests arrive concurrently and multiple API instances are running?

---

## ✨ Quick Features

* ✅ **Token Bucket Rate Limiter** — Custom implementation from first principles
* ✅ **Multiple Algorithms** — Token Bucket, Fixed Window, Sliding Window and Concurrency Limiter
* ✅ **.NET RateLimiter Integration** — Uses `System.Threading.RateLimiting`
* ✅ **Configurable Policies** — Different limits for endpoints, users, API keys and tenants
* ✅ **Batch Processing** — Process large workloads with controlled throughput
* ✅ **Async Processing** — `async/await`, `CancellationToken` and `ValueTask`
* ✅ **Bounded Concurrency** — `Parallel.ForEachAsync` and configurable worker limits
* ✅ **Backpressure** — Bounded `Channel<T>` producer/consumer pipeline
* ✅ **Background Workers** — `BackgroundService` based processing
* ✅ **Distributed Rate Limiting** — Redis-backed shared rate-limit state
* ✅ **Atomic Redis Operations** — Redis transactions/Lua scripts for consistency
* ✅ **HTTP 429 Handling** — `Retry-After` and RateLimit response headers
* ✅ **ProblemDetails** — Standardized API error responses
* ✅ **Thread Safety** — `ConcurrentDictionary`, `Interlocked`, `SemaphoreSlim`
* ✅ **Real-time Metrics** — Request, rejection, queue and throughput metrics
* ✅ **OpenTelemetry** — Metrics, traces and structured observability
* ✅ **Health Checks** — Liveness and readiness endpoints
* ✅ **Failure Handling** — Fail-open/fail-closed strategies
* ✅ **Unit & Integration Tests** — Including concurrency and distributed scenarios
* ✅ **Performance Testing** — BenchmarkDotNet + k6/NBomito
* ✅ **Docker Support** — API + Redis distributed environment
* ✅ **CI/CD** — GitHub Actions build and test pipeline



# 🎯 Project Overview

Rate limiting is used to protect APIs and downstream services from excessive traffic.

A simple implementation can look like:

```text
Request
   ↓
Check Token
   ↓
Token Available?
   ├── Yes → Process Request
   └── No  → Reject / Queue
```

But real production systems introduce additional problems:

* What happens when 10,000 requests arrive
