using MudBlazor;

namespace Ehrms.Web.Extensions;

public static class DateTimeExtensions
{
    public static DateTime? UpdateTime(this DateTime? dateTime, TimeSpan? timeSpan)
    {
        if (dateTime == null || timeSpan == null)
        {
            return dateTime;
        }

        var date = DateOnly.FromDateTime((DateTime)dateTime);
        var time = TimeOnly.FromTimeSpan((TimeSpan)timeSpan);
        
        return new DateTime(date, time);
    }

    public static DateTime? UpdateDate(this DateTime? dateTimeToUpdate, DateTime? newDate)
    {
        if (newDate != null && dateTimeToUpdate == null)
        {
            return newDate;
        }

        if (newDate == null || dateTimeToUpdate == null)
        {
            return null;
        }
        
        var time = TimeOnly.FromDateTime((DateTime)dateTimeToUpdate);
        var date = DateOnly.FromDateTime((DateTime)newDate);
        return new DateTime(date, time);
    }
}