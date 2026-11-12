using CsvHelper.Configuration.Attributes;

namespace WoosterHistoryBooklet.Models;

public class ConvertsDataModel
{
    [Name("Timestamp")]
    public DateTime TimeStamp { get; set; }

    [Name("Your Full Name")]
    public required string FullName { get; set; }

    [Name("Your Baptism Year (optional)")]
    public int? BaptismYear { get; set; }

    [Name("Your Baptism Location (optional)")]
    public string? BaptismLocation { get; set; }

    [Name("Your Story")]
    public required string ConversionStory { get; set; }

    [Name("Photo of you at your baptism (optional)")]
    public string? BaptismPhoto { get; set; }

    [Name("Photo of you today (optional)")]
    public string? TodayPhoto { get; set; }
}