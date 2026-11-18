
using CsvHelper.Configuration.Attributes;

namespace WoosterHistoryBooklet.Models;

public class IndividualDataModel
{
    [Name("Timestamp")]
    public DateTime TimeStamp { get; set; }

    [Name("Full Name")]
    public required string FullName { get; set; }

    [Name("Year You Joined the Wooster Ward")]
    public int YearJoinedWard { get; set; }

    [Name("Year You Left the Wooster Ward (optional)")]
    public int? YearLeftWard { get; set; }

    [Name("Education (optional)")]
    public string? Education { get; set; }

    [Name("Career (optional)")]
    public string? Career { get; set; }

    [Name("Hobbies and Interests (optional)")]
    public string? HobbiesAndInterests { get; set; }

    [Name("Callings in the Wooster Ward (optional)")]
    public string? WardCallings { get; set; }

    [Name("Personal Biography")]
    public required string PersonalBiography { get; set; }

    [Name("Your Photograph (optional)")]
    public string? ProfilePhotograph { get; set; }
}
