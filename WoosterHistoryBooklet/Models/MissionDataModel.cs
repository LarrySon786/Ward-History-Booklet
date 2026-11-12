using CsvHelper.Configuration.Attributes;

namespace WoosterHistoryBooklet.Models;

public class MissionDataModel
{
    [Name("Timestamp")]
    public DateTime TimeStamp { get; set; }

    [Name("Your Full Name")]
    public required string FullName { get; set; }

    [Name("Mission Name")]
    public required string MissionName { get; set; }

    [Name("Year You Left on Your Mission")]
    public int? YearLeft { get; set; }

    [Name("Major City of Your Mission")]
    public required string MajorCity { get; set; }

    [Name("A Photo of You on Your Mission (Optional)")]
    public string? YourProfilePhoto { get; set; }

    [Name("A Photo of Your Mission")]
    public string? MissionPhoto { get; set; }

    [Name("Describe your mission (optional)")]
    public string? MissionDescribed { get; set; }

    [Name("Interesting Story Title")]
    public string? InterestingStoryTitle { get; set; }

    [Name("An Interesting Story")]
    public string? InterestingStory { get; set; }

    [Name("Spiritual Story Title")]
    public string? SpiritualStoryTitle { get; set; }

    [Name("A Spiritual Story")]
    public string? SpiritualStory { get; set; }

    [Name("Number of Baptism (optional)")]
    public int? NumberOfBaptisms { get; set; }
}