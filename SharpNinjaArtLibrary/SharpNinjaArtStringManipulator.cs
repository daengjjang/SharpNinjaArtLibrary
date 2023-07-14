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

using static SharpNinjaArtLibrary.SharpNinjaArtEnum;

namespace SharpNinjaArtLibrary;
public static class SharpNinjaArtStringManipulator
{
    public static IEnumerable<ReadOnlyMemory<char>> Split2Memory(this string text, string pattern)
    {
        var textMemory = text.AsMemory();
        var patternLength = pattern.Length;
        var startIndex = 0;

        foreach (var matchIndex in FindPattern(text, pattern))
        {
            yield return textMemory[startIndex..matchIndex];
            startIndex = matchIndex + patternLength;
        }

        yield return textMemory[startIndex..];
    }
    
    public static IEnumerable<ReadOnlyMemory<char>> BetweenString(this string text, string start, string end)
    {
        var textMemory = text.AsMemory();
        using var startIndices = text.FindPattern(start).GetEnumerator();
        using var endIndices = text.FindPattern(end).GetEnumerator();

        var hasEnd = endIndices.MoveNext();
        while (startIndices.MoveNext())
        {
            var startIndex = startIndices.Current;

            while (hasEnd && startIndex > endIndices.Current)
                hasEnd = endIndices.MoveNext();

            if (!hasEnd)
                break;

            var endIndex = endIndices.Current;
            hasEnd = endIndices.MoveNext();

            yield return textMemory[(startIndex + 1)..endIndex];
        }
    }
    
    public static bool ContainsAndOr(this string text, OrAndEnum comType, StringComparison comparisonType = StringComparison.Ordinal, params string[] strArray) //단순 아호코라식 사용하면 더 빠를듯함
    {
        var comTypeBool = comType is OrAndEnum.Or;
        foreach (var target in strArray)
        {
            if (text.Contains(target, comparisonType) == comTypeBool)
                return comTypeBool;
        }

        return !comTypeBool;
    }
    
    public static string Reverse(this string text) =>
        string.Create(text.Length, text, (chars, state) =>
        {
            state.CopyTo(chars);
            chars.Reverse();
        });
    
    private static IEnumerable<int> FindPatternIndexOf(string input, string pattern)
    {
        var startIndex = 0;
        while (startIndex < input.Length)
        {
            var position = input.IndexOf(pattern, startIndex, StringComparison.Ordinal);
            if (position == -1)
                break;

            yield return position;
            startIndex = position + 1;
        }
    }
    
    public static IEnumerable<int> FindPattern(this string text, string pattern)
    {
        int textLength = text.Length, patternLength = pattern.Length, activeLength = 0;
        var prefixTable = ComputePrefixFunction(pattern);
        for (var i = 0; i < textLength; i++)
        {
            while (activeLength > 0 && pattern[activeLength] != text[i])
                activeLength = prefixTable[activeLength - 1];

            if (pattern[activeLength] == text[i])
                activeLength++;

            if (activeLength != patternLength) 
                continue;
            
            activeLength = prefixTable[activeLength - 1];
            
            yield return i - patternLength + 1;
        }

        static int[] ComputePrefixFunction(string pattern)
        {
            int patternLength = pattern.Length ,activeLength = 0;
            var prefixTable = new int[patternLength];
            for (var i = 1; i < patternLength; i++)
            {
                while (activeLength > 0 && pattern[activeLength] != pattern[i])
                    activeLength = prefixTable[activeLength - 1];

                if (pattern[activeLength] == pattern[i])
                    activeLength++;

                prefixTable[i] = activeLength;
            }

            return prefixTable;
        }
    }
}
