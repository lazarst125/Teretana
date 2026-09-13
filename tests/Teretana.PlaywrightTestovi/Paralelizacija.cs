// Svaka klasa ima sopstvenu hostovanu aplikaciju, pa klase idu paralelno; testovi unutar klase
// dele browser/aplikaciju i izvršavaju se redom, kako Playwright preporučuje za NUnit.
[assembly: Parallelizable(ParallelScope.Fixtures)]
