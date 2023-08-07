namespace SharpNinjaArtLibrary;

public static class SharpNinjaArtItertools
{
    public static IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> list, int length)
    {
        var enumerable = list.ToList();
        return length == 0 ? 
            new []{ Array.Empty<T>() } :
            enumerable.SelectMany((e, i) => 
                Permutations(enumerable.Where((_, j) => i != j), length - 1)
                    .Select(s => new[] { e }.Concat(s))
                );
    }
    public static IEnumerable<IEnumerable<T>> Combinations<T>(IEnumerable<T> list, int length, bool allowReplacement)
    {
        var enumerable = list.ToList();
        return length == 0 ? 
            new[] {Array.Empty<T>()} :
            enumerable.SelectMany((e, i) =>
                Combinations(enumerable.Skip(i + (allowReplacement ? 0 : 1) ), length - 1, allowReplacement)
                    .Select(s => new[] {e}.Concat(s)));
    }
}
