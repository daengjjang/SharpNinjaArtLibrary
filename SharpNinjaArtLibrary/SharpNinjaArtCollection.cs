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

using System.Runtime.InteropServices;

namespace SharpNinjaArtLibrary;
public static class SharpNinjaArtCollection
{
    public static void Shuffle<T>(this IList<T> source) //Sattolo Shuffle
    {
        ArgumentNullException.ThrowIfNull(source);

        for (var i = 0; i < source.Count - 1; i++)
        {
            var rnd = Random.Shared.Next(i, source.Count);
            (source[i], source[rnd]) = (source[rnd], source[i]);
        }
    }
    public static IEnumerable<T> GetRandItems<T>(this IList<T> source, int count)
    {
        ArgumentNullException.ThrowIfNull(source);

        foreach (var item in SharpNinjaArtNumber.IntRandomShuffle(0, source.Count, count))
            yield return source[item];
    }
    public static Span<T> AsSpan<T>(this List<T> source) =>
        CollectionsMarshal.AsSpan(source);
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source) => 
        source is null || source.Any() is false;
    public static (List<T>, List<T>) Separation<T>(this IEnumerable<T> source, Func<T,bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        var (result, result2) = (new List<T>(), new List<T>());
        foreach (var item in source)
        {
            if (predicate(item))
                result.Add(item);
            else
                result2.Add(item);
        }

        return (result, result2);

        //Lookup은 성능적으로 느림!
    }
    public static IEnumerable<T> Cycle<T>(this IEnumerable<T> source)
    {
        var enumerable = source.ToList();
        while (true)
        {
            foreach (var item in enumerable)
            {
                yield return item;
            }
        }
        
        // ReSharper disable once IteratorNeverReturns
    }
}
