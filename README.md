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
|     Method | N |      Mean |     Error |    StdDev |      Gen0 |      Gen1 |      Gen2 | Allocated |
|----------- |-- |----------:|----------:|----------:|----------:|----------:|----------:|----------:|
| Benchmark1 | 1 |  7.439 ms | 0.0821 ms | 0.0768 ms |  468.7500 |  453.1250 |  453.1250 |      8 MB |
| Benchmark2 | 1 |  5.036 ms | 0.0993 ms | 0.2263 ms | 1984.3750 | 1968.7500 | 1968.7500 |      8 MB |
| Benchmark3 | 1 | 13.015 ms | 0.0998 ms | 0.0885 ms |  687.5000 |  671.8750 |  671.8750 |  11.82 MB |

![image](https://github.com/dangjjang/SharpNinjaArtLibrary/assets/139039103/851356f9-e70e-457a-a875-57b707711610)

[stackoverflow](https://stackoverflow.com/questions/30232171/do-functions-slow-down-performance)

```
A method call will always slow you down. 
```


