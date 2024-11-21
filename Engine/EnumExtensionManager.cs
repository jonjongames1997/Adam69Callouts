namespace Adam69Callouts.Engine
{
    // Credit to AstroBurgers and Khorio. github.com/AstroBurgers/RiskierTrafficStops/blob/master/RiskierTrafficStops/Engine/Helpers/Extensions/EnumExtensions.cs

    internal static class EnumExtensionManager
    {
        internal static T PickRandom<T>(this IEnumerable<T> source) => source.Any() ? source.PickRandom(1).Single() : default;

        internal static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count) => source.Shuffle().Take(count);

        internal static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) => source.OrderBy(_ => Guid.NewGuid());
    }
}
