<!-- Name -->
<h1 align="center">
  ✨ inline-serialization ✨
</h1>
<p align="center">
  <a href="#">
    <img alr="MIT License" src="http://img.shields.io/:license-MIT-blue.svg">
  </a>
  <a href="https://t.me/ivysola">
    <img alt="Telegram" src="https://img.shields.io/badge/Ask%20Me-Anything-1f425f.svg">
  </a>
<a href="https://app.fossa.io/projects/git%2Bgithub.com%2FElementaryStudio%2FQuark?ref=badge_shield" alt="FOSSA Status"><img src="https://app.fossa.io/api/projects/git%2Bgithub.com%2FElementaryStudio%2FQuark.svg?type=shield"/></a>
</p>
<p align="center">
  <a href="#">
    <img src="https://forthebadge.com/images/badges/made-with-c-sharp.svg">
    <img src="https://forthebadge.com/images/badges/designed-in-ms-paint.svg">
    <img src="https://forthebadge.com/images/badges/ages-18.svg">
    <img src="https://ForTheBadge.com/images/badges/winter-is-coming.svg">
    <img src="https://forthebadge.com/images/badges/gluten-free.svg">
  </a>
</p>

### Structure

```csharp
Serialize(12f) -> "f2012"

- type header -
f  - float
20 - 32 in hex
- body -
12 - value
```

### Usage


```csharp
var result = FastSerializer.Serialize("test-string"); // s00test-string
var result = FastSerializer.Serialize(12f); // f2012
```



## Benchmark

```
// * Summary *

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.20226
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100-rc.1.20452.10
  [Host]     : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT  [AttachedDebugger]
  DefaultJob : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT


|    Method |     Mean |     Error |    StdDev |
|---------- |---------:|----------:|----------:|
| InlineSer | 1.646 us | 0.0327 us | 0.0468 us |
|       Bin | 7.245 us | 0.1425 us | 0.2341 us |
|      Json | 1.689 us | 0.0297 us | 0.0318 us |

```
