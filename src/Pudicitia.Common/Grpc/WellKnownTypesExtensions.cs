namespace Google.Protobuf.WellKnownTypes;

public static class WellKnownTypesExtensions
{
    public static DateOnly ToDateOnly(this Timestamp timestamp)
    {
        return DateOnly.FromDateTime(timestamp.ToDateTime());
    }

    public static Timestamp ToTimestamp(this DateOnly dateOnly)
    {
        return dateOnly.ToDateTime(TimeOnly.MinValue).ToTimestamp();
    }
}
