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

namespace SharpNinjaArtLibrary;
public static class SharpNinjaArtTypeConverter
{
    private static int ToInt(this string text) => 
        int.TryParse(text.AsSpan(), out var result) ? result : int.MinValue;
    public static int ToInt(this object objData) =>
        objData.ToString()?.ToInt() ?? int.MinValue;
    
    private static long ToInt64(this string text) =>
        long.TryParse(text.AsSpan(), out var result) ? result : long.MinValue;
    public static long ToInt64(this object objData) =>
        objData.ToString()?.ToInt64() ?? long.MinValue;
    
    private static double ToDouble(this string text) =>
        double.TryParse(text.AsSpan(), out var result) ? result : double.MinValue;
    public static double ToDouble(this object objData) =>
        objData.ToString()?.ToDouble() ?? double.MinValue;
    
    private static decimal ToDecimal(this string text) =>
        decimal.TryParse(text.AsSpan(), out var result) ? result : decimal.MinValue;
    public static decimal ToDecimal(this object objData) =>
        objData.ToString()?.ToDecimal() ?? decimal.MinValue;
}
