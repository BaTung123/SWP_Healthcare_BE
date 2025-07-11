namespace BDSS.Common.Utils;

public static class DateTimeUtils
{
    private static readonly TimeZoneInfo GmtPlus7 = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

    public static DateTime GetCurrentGmtPlus7()
    {
        return TimeZoneInfo.ConvertTime(DateTime.UtcNow, GmtPlus7);
    }

    public static DateTime ConvertToGmtPlus7(DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Unspecified)
        {
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
        }
        return TimeZoneInfo.ConvertTime(dateTime, GmtPlus7);
    }

    public static DateTime ConvertFromGmtPlus7(DateTime gmtPlus7DateTime)
    {
        return TimeZoneInfo.ConvertTime(gmtPlus7DateTime, TimeZoneInfo.Utc);
    }
}