namespace FinalYearProject.Infrastructure.Infrastructure.Utilities.DataExtension;

public static class DataExtensions
{


    public static bool IsAny<T>(this IEnumerable<T>? data)
    {
        return data is not null && data.Any();
    }

    



}
