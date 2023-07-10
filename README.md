<div align=center>
  
# SharpNinjaArtLibrary

ChatGpt가 추천해 준 멋있는 이름 SharpNinjaArtLibrary 해당 클래스 라이브러리는 자주 사용하는 기능을 모아뒀어요.

그리고 자주 사용은 안 하지만 Benchmark를 위해 모아둔 기능도 있어요!

**간단하게 코딩 스타일을 확인하실 수 있어요!**

**계속 수정 중입니다!**

## 사용 Tools
![visualstudio](https://img.shields.io/badge/visualstudio-5C2D91?style=&logo=visualstudio&logoColor=#5C2D91)
![visualstudiocode](https://img.shields.io/badge/visualstudiocode-007ACC?style=&logo=visualstudiocode&logoColor=#007ACC)
![rider](https://img.shields.io/badge/rider-000000?style=&logo=rider&logoColor=#000000)

## Benchmark
**BenchmarkDotNet (0.13.5)**

**dotTrace 2023.1.3**
</div>

---

## 클래스 설명

### SharpNinjaArtCollection - Collection 관련 함수들을 모아놨습니다.

## Shuffle
배열을 섞기위해 만들어진 제네릭 메소드입니다.

**Benchmark**

```C#
private IEnumerable<int> array = Enumerable.Range(0,1000000);

[Benchmark]
public void Benchmark1()
{
    var localarray = array.ToList();
    localarray.Shuffle();
}

[Benchmark]
public void Benchmark2()
{
    var localarray = array.OrderBy(_ => Random.Shared.NextDouble()).ToList();
}

[Benchmark]
public void Benchmark3()
{
    var localarray = array.OrderBy(_ => Guid.NewGuid()).ToList();
}
```

|     Method | N |      Mean |    Error |   StdDev |      Gen0 |      Gen1 |      Gen2 | Allocated |
|----------- |-- |----------:|---------:|---------:|----------:|----------:|----------:|----------:|
| Benchmark1 | 1 |  22.61 ms | 0.363 ms | 0.339 ms |  968.7500 |  968.7500 |  968.7500 |   3.82 MB |
| Benchmark2 | 1 | 180.90 ms | 3.579 ms | 4.654 ms | 1000.0000 | 1000.0000 | 1000.0000 |  19.07 MB |
| Benchmark3 | 1 | 256.04 ms | 4.903 ms | 6.546 ms | 1000.0000 | 1000.0000 | 1000.0000 |   26.7 MB |

![image](https://github.com/dangjjang/SharpNinjaArtLibrary/assets/139039103/04d11e9a-692d-4d5f-b950-a257c64ed126)

## GetRandItems
배열에서 갯수만큼 랜덤으로 가져옵니다.

**Benchmark**

```C#
private int[] array = Enumerable.Range(0,1000000).ToArray();

[Benchmark]
public void Benchmark1()
{
    // 중복된 데이터 안가져옴
    var _ = array.GetRandItems(100000).ToList();
}

[Benchmark]
public void Benchmark2()
{
    // 중복된 데이터 가져옴
    var data = new List<int>(100000);
    for (int i = 0, acount = array.Length; i < 100000; i++)
    {
        data.Add(array[Random.Shared.Next(0,acount)]);
    }
}

[Benchmark]
public void Benchmark3()
{
    // 중복된 데이터 안가져옴
    var hash = new HashSet<int>();
    var data = new List<int>(100000);
    for (int i = 0, acount = array.Length; i < 100000; i++)
    {
        var randData = Random.Shared.Next(0, acount);
        if (hash.Add(randData))
            data.Add(randData);
    }
}
```
|     Method | N |      Mean |     Error |    StdDev |     Gen0 |     Gen1 |     Gen2 |  Allocated |
|----------- |-- |----------:|----------:|----------:|---------:|---------:|---------:|-----------:|
| Benchmark1 | 1 | 25.866 ms | 0.5156 ms | 0.7874 ms | 593.7500 | 593.7500 | 593.7500 | 4931.04 KB |
| Benchmark2 | 1 |  1.195 ms | 0.0233 ms | 0.0303 ms |  83.9844 |  83.9844 |  83.9844 |  390.71 KB |
| Benchmark3 | 1 |  4.259 ms | 0.0827 ms | 0.0919 ms | 476.5625 | 468.7500 | 468.7500 | 5107.95 KB |

```
주의: Benchmark3는 이론적으로 무한 루프에 빠질 수 있는 가능성이 있습니다.
이를 방지하기 위해, Benchmark1은 GetRandItems메소드 내에서 IntRandomShuffle을 사용하였습니다.
만약 무한 루프의 위험을 감수하고 사용하고자 한다면, IntRandomShuffle 메소드를 IntRandomHashSet으로 변경하면 됩니다.
```
![image](https://github.com/dangjjang/SharpNinjaArtLibrary/assets/139039103/2312c987-3e12-409c-a815-f90887264da2)

## AsSpan
List를 간편하게 Span을 만들기위한 메소드입니다.
```C#
var _ = new List<int> {1, 2, 3, 4, 5}.AsSpan();
```

## IsNullOrEmpty
해당 컬렉션이 Null이거나 비어있는지 확인합니다.
```C#
var _ = new List<int> {1, 2, 3, 4, 5}.IsNullOrEmpty();
```

## Separation
해당 컬렉션을 두개로 나눠줍니다. 예시 ) 홀수 짝수

**Benchmark**
```C#
private int[] array = Enumerable.Range(0,1000000).ToArray();

[Benchmark]
public void Benchmark1()
{
    var (o,t) =  array.Separation(i => i % 2 == 0);
}

[Benchmark]
public void Benchmark2()
{
    var o = array.Where(w => w % 2 == 0).ToList();
    var t = array.Where(w => w % 2 != 0).ToList();
}

[Benchmark]
public void Benchmark3()
{
    var data = array.ToLookup(i => i % 2 == 0);
    var o = data[true].ToList();
    var t = data[false].ToList();
}
```
|     Method |     Mean |     Error |    StdDev | Ratio | RatioSD |      Gen0 |     Gen1 |     Gen2 | Allocated | Alloc Ratio |
|----------- |---------:|----------:|----------:|------:|--------:|----------:|---------:|---------:|----------:|------------:|
| Benchmark1 | 3.955 ms | 0.0741 ms | 0.0728 ms |  1.00 |    0.00 | 1000.0000 | 984.3750 | 984.3750 |      4 MB |        1.00 |
| Benchmark2 | 4.580 ms | 0.0773 ms | 0.0723 ms |  1.16 |    0.02 | 1000.0000 | 984.3750 | 984.3750 |      4 MB |        1.00 |
| Benchmark3 | 8.222 ms | 0.0851 ms | 0.0796 ms |  2.08 |    0.03 |  843.7500 | 828.1250 | 828.1250 |   5.91 MB |        1.48 |

![image](https://github.com/dangjjang/SharpNinjaArtLibrary/assets/139039103/851356f9-e70e-457a-a875-57b707711610)

[stackoverflow](https://stackoverflow.com/questions/30232171/do-functions-slow-down-performance)

```
A method call will always slow you down. 
```

### SharpNinjaArtConvenience - 편리한 기능

## Swap

```C#
Swap(ref a, ref b);
```

```C#
var a = new string('가', 100000000);
var b = new string('나', 100000000);

1.
(a, b) = (b, a);

2.
var temp = "";
temp = b;
b = a;
a = temp;
````

```
사실 두소스는 컴파일시 컴파일러가 자동으로 최적화 해줘서 똑같습니다.
```

```
1번 소스

IL_0000: ldc.i4       44032 // 0x0000ac00
IL_0005: ldc.i4       100000000 // 0x05f5e100
IL_000a: newobj       instance void [System.Runtime]System.String::.ctor(char, int32)
IL_000f: stloc.0      // a

IL_0010: ldc.i4       45208 // 0x0000b098
IL_0015: ldc.i4       100000000 // 0x05f5e100
IL_001a: newobj       instance void [System.Runtime]System.String::.ctor(char, int32)
IL_001f: stloc.1      // b

IL_0020: ldloc.1      // b
IL_0021: ldloc.0      // a
IL_0022: stloc.1      // b
IL_0023: stloc.0      // a

IL_0024: ldloc.0      // a
IL_0025: ldloc.1      // b
IL_0026: call         string [System.Runtime]System.String::Concat(string, string)
IL_002b: pop

IL_002c: ret
```

```
2번 소스

IL_0000: ldc.i4       44032 // 0x0000ac00
IL_0005: ldc.i4       100000000 // 0x05f5e100
IL_000a: newobj       instance void [System.Runtime]System.String::.ctor(char, int32)
IL_000f: stloc.0      // a

IL_0010: ldc.i4       45208 // 0x0000b098
IL_0015: ldc.i4       100000000 // 0x05f5e100
IL_001a: newobj       instance void [System.Runtime]System.String::.ctor(char, int32)
IL_001f: stloc.1      // b

IL_0020: ldloc.1      // b
IL_0021: ldloc.0      // a
IL_0022: stloc.1      // b
IL_0023: stloc.0      // a

IL_0024: ldloc.0      // a
IL_0025: ldloc.1      // b
IL_0026: call         string [System.Runtime]System.String::Concat(string, string)
IL_002b: pop

IL_002c: ret
```

### SharpNinjaArtNumber - 숫자 관련 함수들을 모아놨습니다.

## SimdMin

simd를 지원하는 Vector 를 이용해서 최소값을 찾는 메소드입니다. 

(SIMD(Single Instruction Multiple Data)는 병렬 컴퓨팅의 한 종류로, 하나의 명령어로 여러 개의 값을 동시에 계산하는 방식이다.)
 
**해당소스는 Obsolete 입니다.**

**Benchmark**

```C#
[Benchmark(Baseline = true)]
public void Benchmark1()
{
  var _ = _array.AsSpan().SimdMin();
}

[Benchmark]
public void Benchmark2()
{
  var _ = _array.Min();
}

[Benchmark]
public void Benchmark3()
{
  var min = int.MaxValue;
  foreach (var t in _array)
    min = Math.Min(min, t);
}
```

|     Method |       Mean |    Error |   StdDev |     Median | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |-----------:|---------:|---------:|-----------:|------:|--------:|----------:|------------:|
| Benchmark1 |   313.5 us | 15.06 us | 44.17 us |   294.3 us |  1.00 |    0.00 |         - |          NA |
| Benchmark2 |   359.4 us |  7.04 us | 10.53 us |   355.3 us |  1.10 |    0.15 |         - |          NA |
| Benchmark3 | 1,392.4 us | 27.32 us | 35.52 us | 1,389.1 us |  4.20 |    0.65 |       1 B |          NA |

![image](https://github.com/daengjjang/SharpNinjaArtLibrary/assets/139039103/4b50565b-adfc-4066-a01c-982618b0dc22)

SIMDMin이 더 빠른 성능을 제공할지라도, 여전히 LINQ의 Min 함수를 선호하는 이유는 다음과 같습니다. 

첫째, 두 함수의 성능 차이는 그다지 크지 않습니다. (Min Vector를 사용합니다.)

둘째, LINQ의 Min 함수는 여러 부분에서 더 안정적이라는 장점이 있습니다. 이러한 이유로 인해, 우리는 LINQ의 Min 함수를 선택하는 편입니다.

![image](https://github.com/daengjjang/SharpNinjaArtLibrary/assets/139039103/0b42d876-9056-42ed-8d01-e6e9c67d24be)

1. LINQ의 Min 함수는 예외 처리가 탁월하게 수행되므로 신뢰성이 높습니다. 

2. 또한, 'IsHardwareAccelerated'와 같이, 하드웨어가 특정 기능을 지원하지 않는 상황에 대한 처리 로직이 이미 구현되어 있어, 사용성이 더욱 우수합니다.

## SimdMax

simd를 지원하는 Vector 를 이용해서 최대값을 찾는 메소드입니다. 

**해당소스는 Obsolete 입니다.**

**Benchmark**

```C#
[Benchmark(Baseline = true)]
public void Benchmark1()
{
    var _ = _array.AsSpan().SimdMax();
}

[Benchmark]
public void Benchmark2()
{
    var _ = _array.Max();
}
    
[Benchmark]
public void Benchmark3()
{
    var max = int.MinValue;
    foreach (var t in _array)
        max = Math.Max(max, t);
}
```

|     Method |       Mean |    Error |   StdDev | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |-----------:|---------:|---------:|------:|--------:|----------:|------------:|
| Benchmark1 |   275.6 us |  3.74 us |  3.12 us |  1.00 |    0.00 |         - |          NA |
| Benchmark2 |   422.9 us |  7.48 us |  6.99 us |  1.54 |    0.04 |         - |          NA |
| Benchmark3 | 1,347.7 us | 13.89 us | 10.84 us |  4.88 |    0.05 |       1 B |          NA |

![image](https://github.com/daengjjang/SharpNinjaArtLibrary/assets/139039103/c954754a-1477-4bb8-b38c-8e5641c53a18)

위와 같은(MIN) 이유로 LINQ MAX를 선호합니다.

## SimdSum

simd를 지원하는 Vector 를 이용해서 배열의 합을 구하는 메소드입니다. 

**Benchmark**

```C#
[Benchmark(Baseline = true)]
public void Benchmark1()
{
    var _ = _array.AsSpan().SimdSum();
}

[Benchmark]
public void Benchmark2()
{
    var _ = _array.Sum();
}
    
[Benchmark]
public void Benchmark3()
{
    var hap = 0;
    foreach (var t in _array)
        hap += t;
}
```

|     Method |      Mean |     Error |    StdDev | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------- |----------:|----------:|----------:|------:|--------:|----------:|------------:|
| Benchmark1 |  1.786 us | 0.0347 us | 0.0474 us |  1.00 |    0.00 |         - |          NA |
| Benchmark2 | 11.585 us | 0.2230 us | 0.3127 us |  6.49 |    0.26 |         - |          NA |
| Benchmark3 | 11.540 us | 0.1961 us | 0.2480 us |  6.46 |    0.20 |         - |          NA |

LINQ Sum은 Vector을 이용하지않습니다. 하지만 예외처리, 오버플로우 체크등을 해줍니다.

![image](https://github.com/daengjjang/SharpNinjaArtLibrary/assets/139039103/8d89ef7e-2be9-4c89-b059-5c22bf3552d5)

## SimdSum

확률로 Bool 타입을 반환해주는 메소드입니다.

```C#
  RandomBool() // 50% 확률로 True 반환
```

## IntRandomShuffle, IntRandomHashSet

최솟값 최댓값 사이에 숫자 중에 지정한 개수만큼 랜덤으로 반환하는 메소드입니다.

IntRandomShuffle : 최소값 최대값 사이의 모든 숫자를 배열로 만든다음 Shuffle 메소드를 실행시킨후 지정한 개수만큼 반환합니다.

IntRandomHashSet : Random을 이용하여 랜덤 숫자를 추출 후 Hashset을 이용해 중복이 아닌경우 반환합니다.

**Benchmark**

```C#
[Benchmark(Baseline = true)]
public void Benchmark1()
{
    var _ = SharpNinjaArtNumber.IntRandomShuffle(0, 1000000, 900000).ToList();
}

[Benchmark]
public void Benchmark2()
{
    var _ = SharpNinjaArtNumber.IntRandomHashSet(0, 1000000, 900000).ToList();
}
}
```

1 번쨰

|     Method |                Mean |             Error |            StdDev |              Median | Ratio | RatioSD |      Gen0 |      Gen1 |      Gen2 |  Allocated | Alloc Ratio |
|----------- |--------------------:|------------------:|------------------:|--------------------:|------:|--------:|----------:|----------:|----------:|-----------:|------------:|
| Benchmark1 |  25,873,179.5455 ns |   498,417.9736 ns |   612,102.0323 ns |  25,835,853.1250 ns | 1.000 |    0.00 |  812.5000 |  812.5000 |  812.5000 |  7600470 B |        1.00 |
| Benchmark2 | 119,116,535.0000 ns | 2,360,340.1703 ns | 6,959,516.2468 ns | 117,594,112.5000 ns | 4.727 |    0.29 | 1000.0000 | 1000.0000 | 1000.0000 | 51501044 B |        6.78 |

2 번째

|     Method |                Mean |             Error |            StdDev | Ratio | RatioSD |      Gen0 |      Gen1 |      Gen2 |  Allocated | Alloc Ratio |
|----------- |--------------------:|------------------:|------------------:|------:|--------:|----------:|----------:|----------:|-----------:|------------:|
| Benchmark1 |  26,349,054.4643 ns |   343,023.4810 ns |   304,081.3812 ns | 1.000 |    0.00 |  281.2500 |  281.2500 |  281.2500 |  7600310 B |        1.00 |
| Benchmark2 | 119,371,837.1831 ns | 2,370,389.3289 ns | 5,814,605.4169 ns | 4.717 |    0.21 | 1400.0000 | 1400.0000 | 1400.0000 | 51501142 B |        6.78 |

![image](https://github.com/daengjjang/SharpNinjaArtLibrary/assets/139039103/8a343147-43b1-481d-a986-ab84b071e5ae)

Count가 적을시

```C#
[Benchmark(Baseline = true)]
public void Benchmark1()
{
    var _ = SharpNinjaArtNumber.IntRandomShuffle(0, 1000000, 100).ToList();
}

[Benchmark]
public void Benchmark2()
{
    var _ = SharpNinjaArtNumber.IntRandomHashSet(0, 1000000, 100).ToList();
}
```

|     Method |               Mean |           Error |          StdDev |             Median | Ratio |     Gen0 |     Gen1 |     Gen2 | Allocated | Alloc Ratio |
|----------- |-------------------:|----------------:|----------------:|-------------------:|------:|---------:|---------:|---------:|----------:|------------:|
| Benchmark1 | 23,366,397.5446 ns | 387,039.2527 ns | 343,100.2163 ns | 23,450,384.3750 ns | 1.000 | 500.0000 | 500.0000 | 500.0000 | 4000772 B |       1.000 |
| Benchmark2 |      4,494.0144 ns |      87.0495 ns |     135.5256 ns |      4,462.8220 ns | 0.000 |   0.4501 |   0.0076 |        - |    7560 B |       0.002 |

![image](https://github.com/daengjjang/SharpNinjaArtLibrary/assets/139039103/d52a3ace-4d28-4074-bfb3-c789ea13b508)
