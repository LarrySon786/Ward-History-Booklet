using CsvHelper.Configuration.Attributes;

namespace WoosterHistoryBooklet.Models;

public class FamilyDataModel
{
    [Name("Timestamp")]
    public DateTime TimeStamp { get; set; }

    [Name("Family Surname")]
    public required string FamilySurname { get; set; }

    [Name("Father's Full Name (optional)")]
    public string? FatherFullName { get; set; }

    [Name("Mother's Full Name (optional)")]
    public string? MotherFullName { get; set; }

    [Name("Family Biography")]
    public required string FamilyBiography { get; set; }

    [Name("Temple (optional)")]
    public string? Temple { get; set; }

    [Name("Why Is This Temple Meaningful to Your Family? (optional)")]
    public string? TempleMeaning { get; set; }

    [Name("Father's Biography (optional)")]
    public string? FatherBiography { get; set; }

    [Name("Mother's Biography (Optional)")]
    public string? MotherBiography { get; set; }

    [Name("Story's Title (optional)")]
    public string? StoryTitle { get; set; }

    [Name("In a galaxy far far away... (optional)")]
    public string? FamilyStory { get; set; }

    [Name("Child 1")]
    public string? Child1 { get; set; }

    [Name("Child 2")]
    public string? Child2 { get; set; }

    [Name("Child 3")]
    public string? Child3 { get; set; }

    [Name("Child 4")]
    public string? Child4 { get; set; }

    [Name("Child 5")]
    public string? Child5 { get; set; }

    [Name("Child 6")]
    public string? Child6 { get; set; }

    [Name("Child 7")]
    public string? Child7 { get; set; }

    [Name("Child 8")]
    public string? Child8 { get; set; }

    [Name("Child 9")]
    public string? Child9 { get; set; }

    [Name("Child 10")]
    public string? Child10 { get; set; }

    [Name("Child 11")]
    public string? Child11 { get; set; }

    [Name("Child 12")]
    public string? Child12 { get; set; }

    [Name("Child 13")]
    public string? Child13 { get; set; }

    [Name("Child 14")]
    public string? Child14 { get; set; }

    [Name("Child 15")]
    public string? Child15 { get; set; }

    [Name("Family Photograph (Optional)")]
    public string? FamilyPhotograph { get; set; }

    [Name("Temple Photograph (optional)")]
    public string? TemplePhotograph { get; set; }

    [Name("Year You Joined the Wooster Ward")]
    public int? YearJoinedWard { get; set; }

    [Name("Year You Left the Wooster Ward (optional)")]
    public int? YearLeftWard { get; set; }

    [Name("Wedding Photo or Alternate Photo (optional)")]
    public string? WeddingOrAlternatePhoto { get; set; }

    // Calculated Property
    [Ignore]
    public List<string> Children =>
    new List<string?>
    {
        Child1,
        Child2,
        Child3,
        Child4,
        Child5,
        Child6,
        Child7,
        Child8,
        Child9,
        Child10,
        Child11,
        Child12,
        Child13,
        Child14,
        Child15
    }
    .Where(child => !string.IsNullOrWhiteSpace(child))
    .Select(child => child!.Trim())
    .ToList();
}