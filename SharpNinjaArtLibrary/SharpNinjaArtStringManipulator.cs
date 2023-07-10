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
    public static List<string> Split(ReadOnlySpan<char> text, ReadOnlySpan<char> pattern)
    {
        var result = new List<string>();
        var findIndex = new Queue<int>(FindPattern(text, pattern));
        var startIndex = 0;
        while (findIndex.Count > 0)
        {
            var endIndex = findIndex.Dequeue();
            result.Add(text[startIndex..endIndex].ToString());
            startIndex = endIndex + 1;
        }

        result.Add(text[startIndex..text.Length].ToString());
        return result;
    }
    public static List<string> BetweenString(this ReadOnlySpan<char> text, ReadOnlySpan<char> start, ReadOnlySpan<char> end)
    {
        var result = new List<string>();
        var startQueue = new Queue<int>(text.FindPattern(start));
        var endQueue = new Queue<int>(text.FindPattern(end));
        while (startQueue.Count > 0 && endQueue.Count > 0)
        {
            var startIndex = startQueue.Dequeue();
            while (endQueue.Count > 0 && startIndex > endQueue.Peek()) 
                endQueue.Dequeue();

            if (endQueue.Count <= 0) 
                break;
            
            result.Add(text[(startIndex+1)..endQueue.Dequeue()].ToString());
        }

        return result;
    }
    public static bool ContainsAndOr(this ReadOnlySpan<char> text, OrAndEnum comType, StringComparison comparisonType = StringComparison.Ordinal, params string[] strArray) //단순 아호코라식 사용하면 더 빠를듯함
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

}
