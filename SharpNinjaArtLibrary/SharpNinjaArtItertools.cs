namespace SharpNinjaArtLibrary;

public static class SharpNinjaArtItertools
{
    public static IEnumerable<IEnumerable<T>> Product<T>(IEnumerable<T> list, int repeat)
    {
        var enumerable = list.ToList();
        return Enumerable.Repeat(enumerable, repeat).
            Aggregate(
                new[] { Enumerable.Empty<T>() } as IEnumerable<IEnumerable<T>>, 
                (accumulator, seq) => 
                    accumulator.SelectMany(_ => seq, (e, seq) => e.Concat(new[] { seq })));
    }

    public static IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> list, int length)
    {
        var enumerable = list.ToList();
        return length == 0 ? 
            new []{ Array.Empty<T>() } :
            enumerable.SelectMany((_, i) => 
                    Permutations(enumerable.Where((_, j) => i != j), length - 1) , 
                (e ,per) => new [] {e}.Concat(per)
            );
    }

    public static IEnumerable<IEnumerable<T>> Combinations<T>(IEnumerable<T> list, int length, bool allowReplacement)
    {
        var enumerable = list.ToList();
        return length == 0
            ? new[] {Array.Empty<T>()}
            : enumerable.SelectMany((_, i) =>
                    Combinations(enumerable.Skip(i + (allowReplacement ? 0 : 1)), length - 1, allowReplacement),
                (e, comb) => new[] {e}.Concat(comb)
            );
    }
}
