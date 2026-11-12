using System.Text.RegularExpressions;
using WoosterHistoryBooklet.Extensions;
using WoosterHistoryBooklet.Models;
using WoosterHistoryBooklet.Services;

namespace WoosterHistoryBooklet.PageGenerators;

public static class MissionPageGenerator
{
    

    public static async Task<string> GeneratePages(
        List<MissionDataModel> missionData,
        ImageHelperService imageHelper,
        ImageSelectionService imageSelectionService)
    {
        // Create the entire html section for missionaries
        string masterMissionHtml = string.Empty;

        foreach (var data in missionData)
        {
            // Create a new individual missionary section for each missionary
            string missionHtml = await File.ReadAllTextAsync("HtmlTemplates/Missionary.html");

            // Download Images from Google Drive if not already downloaded:
            var imageOneReference = await imageHelper.DownloadImageAsync(
                data.YourProfilePhoto,
                $"{data.FullName}-Your Photo on mission - {ImageNameExtension.RemoveSlashesAndColons(data.TimeStamp.ToString())}"
            );
            var imageTwoReference = await imageHelper.DownloadImageAsync(
                data.MissionPhoto, 
                $"{data.FullName}-Mission Photo - {ImageNameExtension.RemoveSlashesAndColons(data.TimeStamp.ToString())}"
            );

            // Use placeholder image if no image was uploaded
            imageOneReference ??= imageSelectionService.GetNextImageReference(ImageSubject.MissionProfile);
            imageTwoReference ??= imageSelectionService.GetNextImageReference(ImageSubject.MissionLandscape);

            // Ensure no paragraph spacing in stories
            data.SpiritualStory = Regex.Replace(data.SpiritualStory ?? "", @"\s+", " ").Trim();
            data.InterestingStory = Regex.Replace(data.InterestingStory ?? "", @"\s+", " ").Trim();
            data.MissionDescribed = Regex.Replace(data.MissionDescribed ?? "", @"\s+", " ").Trim();


            // Replace all html template placeholder data with REAL data from missionaries
            missionHtml = missionHtml.Replace("{{imageOneReference}}", imageOneReference);
            missionHtml = missionHtml.Replace("{{imageTwoReference}}", imageTwoReference);
            missionHtml = missionHtml.Replace("{{YearLeft}}", data.YearLeft.ToString());
            missionHtml = missionHtml.Replace("{{FullName}}", data.FullName);
            missionHtml = missionHtml.Replace("{{MissionName}}", data.MissionName);
            missionHtml = missionHtml.Replace("{{MissionStartYear}}", data.YearLeft.ToString());
            missionHtml = missionHtml.Replace(
                "{{AboutMissionStyle}}",
                FontSizeService.GetFittedFontSizeStyle(data.MissionDescribed, 7.38f, 5.4f));
            missionHtml = missionHtml.Replace("{{MissionDescribed}}", data.MissionDescribed);
            missionHtml = missionHtml.Replace("{{InterestingStoryTitle}}", data.InterestingStoryTitle);
            missionHtml = missionHtml.Replace( "{{InterestingStory}}", data.InterestingStory);
            missionHtml = missionHtml.Replace(
                "{{InterestingStoryStyle}}",
                FontSizeService.GetFittedFontSizeStyle(data.InterestingStory, 7.38f, 2.5f));
            missionHtml = missionHtml.Replace("{{SpiritualStoryTitle}}", data.SpiritualStoryTitle);
            missionHtml = missionHtml.Replace(
                "{{SpiritualStory}}",
                data.SpiritualStory);
            missionHtml = missionHtml.Replace(
                "{{SpiritualStoryStyle}}",
                FontSizeService.GetFittedFontSizeStyle(data.SpiritualStory, 7.38f, 2.5f));

            // Append the new html template to the missionary section of booklet
            masterMissionHtml += missionHtml;
        }
        return masterMissionHtml;
    }







}