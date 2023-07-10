#region Copyright (c) 2023 [SharpNinjaArtLibrary]. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion
using System.Numerics;
using System.Runtime.InteropServices;

namespace SharpNinjaArtLibrary;
public static class SharpNinjaArtNumber
{
    [Obsolete("I'll use Linq Min")]
    public static T SimdMin<T>(this Span<T> source) where T : unmanaged, INumber<T>, IMinMaxValue<T>
    {
        if (Vector.IsHardwareAccelerated is false)
            return default;

        var spanAsVectors = MemoryMarshal.Cast<T, Vector<T>>(source);
        var minVector = new Vector<T>(T.MaxValue);

        foreach (var spanAsVector in spanAsVectors)
            minVector = Vector.Min(minVector, spanAsVector);

        var result = T.MaxValue;
        foreach (var data in source[^(source.Length % Vector<T>.Count)..])
            result = T.Min(result, data);

        for (var i = 0; i < Vector<T>.Count; i++)
            result = T.Min(result, minVector[i]);

        return result;
    }
    [Obsolete("I'll use Linq Max")]
    public static T SimdMax<T>(this Span<T> source) where T : unmanaged, INumber<T>, IMinMaxValue<T>
    {
        if (Vector.IsHardwareAccelerated is false)
            return default;

        var spanAsVectors = MemoryMarshal.Cast<T, Vector<T>>(source);
        var maxVector = new Vector<T>(T.MinValue);

        foreach (var spanAsVector in spanAsVectors)
            maxVector = Vector.Max(maxVector, spanAsVector);

        var result = T.MinValue;
        foreach (var data in source[^(source.Length % Vector<T>.Count)..])
            result = T.Max(result, data);

        for (var i = 0; i < Vector<T>.Count; i++)
            result = T.Max(result, maxVector[i]);

        return result;
    }

    public static T SimdSum<T>(this Span<T> source) where T : unmanaged, INumber<T>
    {
        if (Vector.IsHardwareAccelerated is false)
            return default;

        var spanAsVectors = MemoryMarshal.Cast<T, Vector<T>>(source);
        var sumVector = Vector<T>.Zero;

        foreach (var spanAsVector in spanAsVectors)
            sumVector = Vector.Add(sumVector, spanAsVector);

        var result = T.Zero;
        foreach (var data in source[^(source.Length % Vector<T>.Count)..])
            result += data;

        for (var i = 0; i < Vector<T>.Count; i++)
            result += sumVector[i];

        return result;
    }
    public static bool RandomBool(double percentage = 50)
    {
        var seed = Guid.NewGuid().GetHashCode();
        var rand = new Random(seed);
        var divPercentage = percentage / 100;
        return rand.NextDouble() < divPercentage;
    }
    public static IEnumerable<int> IntRandomShuffle(int min, int max, int count)
    {
        if (min >= max)
            throw new ArgumentException("MinMaxParameter Error");

        if (Math.Abs(max - min) < count)
            throw new ArgumentException("Count Rang Error");

        var eumeRange = Enumerable.Range(min, max - min).ToList();
        eumeRange.Shuffle();

        return eumeRange.Take(count);
    }
    public static IEnumerable<int> IntRandomHashSet(int min, int max, int count)
    {
        if (min >= max)
            throw new ArgumentException("MinMaxParameter Error");

        if (Math.Abs(max - min) < count)
            throw new ArgumentException("Count Rang Error");

        var hashData = new HashSet<int>();
        var seed = Guid.NewGuid().GetHashCode();
        var rand = new Random(seed);
        while (hashData.Count < count)
        {
            var randData = rand.Next(min, max);
            if (hashData.Add(randData))
                yield return randData;
        }
    }
}
