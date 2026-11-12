// Read CSV files

using System.Globalization;

namespace WoosterHistoryBooklet.Extensions;

public static class CsvExtension
{
    public static List<T> ReadCsv<T>(string filePath)
    {
        using var reader = new StreamReader(filePath);

        using var csv = new CsvHelper.CsvReader(
            reader,
            CultureInfo.InvariantCulture);

        return csv.GetRecords<T>().ToList();
    }
}