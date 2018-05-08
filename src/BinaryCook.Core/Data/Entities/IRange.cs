namespace BinaryCook.Core.Data.Entities
{
    public interface IRange<T> where T : struct
    {
        T? From { get; }
        T? To { get; }
    }

    public static class RangeExtensions
    {
        public static bool IsDefined<T>(this IRange<T> range) where T : struct => range != null && (range.From != null || range.To != null);
    }
}