namespace FinalYearProject.Infrastructure.Infrastructure.Utilities.DataExtension;

public static class DataExtensions
{


    public static bool IsAny<T>(this IEnumerable<T>? data)
    {
        return data is not null && data.Any();
    }

    public static string ExipireStringTime(this double dateInMins)
    {
        string time = dateInMins <= 0 ? $"" : dateInMins < 1 ? $"few seconds" : dateInMins < 2 ? $"a minute" : dateInMins <= 60 ? $"{(int)dateInMins} minutes" : (int)(dateInMins / 60) < 2 ? $"{(int)(dateInMins / 60)} hour" : $"{(int)(dateInMins / 60)} hours";
        return time;
    }

    public static string GetDatePosted(this double dateInMins)
    {
        string time = dateInMins < 0 ? $"" : dateInMins < 1 ? $"few seconds ago" : dateInMins < 2 ? $"a minute ago" : dateInMins <= 60 ? $"{(int)dateInMins} minutes ago" : (int)(dateInMins / 60) < 2 ? $"{(int)(dateInMins / 60)} hour ago"
            : (int)(dateInMins / 60) <= 24 ? $"{(int)(dateInMins / 60)} hours ago" : (int)(dateInMins / 1440) < 2 ? $"a day ago"
            : (int)(dateInMins / 1440) <= 30 ? $"{(int)(dateInMins / 1440)} days ago" : (int)(dateInMins / 43200) < 2 ? $"{(int)(dateInMins / 43200)} month ago"
            : (int)(dateInMins / 43200) <= 12 ? $"{(int)(dateInMins / 43200)} months ago" : (int)(dateInMins / 518400) < 2 ? $"{(int)(dateInMins / 518400)} year ago" : $"{(int)(dateInMins / 518400)} years ago";
        return time;
    }



}
