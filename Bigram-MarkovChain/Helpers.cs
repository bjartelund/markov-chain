public static class Helpers{
    public static IEnumerable<(T, T)> Pairwise<T>(this IEnumerable<T> source)
    {
        using var it = source.GetEnumerator();
            
        if (!it.MoveNext())
            yield break;

        var previous = it.Current;

        while (it.MoveNext())
            yield return (previous, previous = it.Current);
    }
}