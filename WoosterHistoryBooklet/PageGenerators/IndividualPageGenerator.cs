using System.Text.RegularExpressions;
using WoosterHistoryBooklet.Extensions;
using WoosterHistoryBooklet.Models;
using WoosterHistoryBooklet.Services;

namespace WoosterHistoryBooklet.PageGenerators;

public static class IndividualPageGenerator
{
    public static async Task<string> GeneratePages(
        List<IndividualDataModel> individualData,
        ImageHelperService imageHelper,
        ImageSelectionService imageSelectionService)
    {
        // Create the entire html section for Individuals
        string masterIndividualHtml = string.Empty;

        foreach (var data in individualData)
        {
            // Create a new individual section for each Individual
            string individualHtml = await File.ReadAllTextAsync("HtmlTemplates/Individuals.html");

            // Download Images from Google Drive if not already downloaded:
            var imageOneReference = await imageHelper.DownloadImageAsync(
                data.ProfilePhotograph,
                $"{data.FullName}-Individual Photo - {ImageNameExtension.RemoveSlashesAndColons(data.TimeStamp.ToString())}"
            );

            // Use placeholder image if no image was uploaded
            imageOneReference ??= imageSelectionService.GetNextImageReference(ImageSubject.MissionProfile);

            // Ensure no paragraph spacing in stories
            data.PersonalBiography = Regex.Replace(data.PersonalBiography ?? "", @"\s+", " ").Trim();
            data.Career = Regex.Replace(data.Career ?? "", @"\s+", " ").Trim();
            data.HobbiesAndInterests = Regex.Replace(data.HobbiesAndInterests ?? "", @"\s+", " ").Trim();
            data.Education = Regex.Replace(data.Education ?? "", @"\s+", " ").Trim();
            data.WardCallings = Regex.Replace(data.WardCallings ?? "", @"\s+", " ").Trim();


            // Replace all html template placeholder data with REAL data from Individuals
            individualHtml = individualHtml.Replace("{{ImageOneReference}}", imageOneReference);
            individualHtml = individualHtml.Replace("{{DatesMarkUp}}", GetDatesMarkUp(data));
            individualHtml = individualHtml.Replace("{{FullName}}", data.FullName);
            individualHtml = individualHtml.Replace("{{Education}}", GetShortBioMarkUp("Education", data.Education));
            individualHtml = individualHtml.Replace("{{Career}}", GetShortBioMarkUp("Career", data.Career));
            individualHtml = individualHtml.Replace("{{Hobbies}}", GetShortBioMarkUp("Hobbies", data.HobbiesAndInterests));
            individualHtml = individualHtml.Replace("{{Callings}}", GetShortBioMarkUp("Callings", data.WardCallings));
            individualHtml = individualHtml.Replace("{{PersonalBiography}}", data.PersonalBiography);
            individualHtml = individualHtml.Replace(
                "{{PersonalBiographyStyle}}",
                FontSizeService.GetFittedFontSizeStyle(data.PersonalBiography, 7.38f, 5.4f));


            // Append the new html template to the Individuals section of booklet
            masterIndividualHtml += individualHtml;
        }
        return masterIndividualHtml;
    }


    private static string GetDatesMarkUp(IndividualDataModel data)
    {
        string dateMarkup = string.IsNullOrWhiteSpace(data.YearLeftWard.ToString())
            ? $"Wooster Ward: {data.YearJoinedWard}"
            : $"Wooster Ward: {data.YearJoinedWard} - {data.YearLeftWard}";

        return dateMarkup;
    }
    private static string GetShortBioMarkUp(string bioName, string shortBio)
    {
        string markUp = string.IsNullOrWhiteSpace(shortBio)
            ? string.Empty
            : $"""
                <div class="individual-bio-cards short-bio-content-container">
                    <div>{bioName}</div>
                    <div>{shortBio}</div>
                </div>
            """;

        return markUp;
    }

}