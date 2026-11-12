using System.Text.RegularExpressions;
using WoosterHistoryBooklet.Extensions;
using WoosterHistoryBooklet.Models;
using WoosterHistoryBooklet.Services;

namespace WoosterHistoryBooklet.PageGenerators;

public static class ConversionsPageGenerator
{

    public static async Task<string> GeneratePages(
        List<ConvertsDataModel> convertData,
        ImageHelperService imageHelper,
        ImageSelectionService imageSelectionService)
    {
        // Create the entire html section for convert
        string masterConvertHtml = string.Empty;

        foreach (var data in convertData)
        {
            // Create a new individual convert section for each convert
            string convertHtml = await File.ReadAllTextAsync("HtmlTemplates/Convert.html");

            // Download Images from Google Drive if not already downloaded:
            var imageOneReference = await imageHelper.DownloadImageAsync(
                data.TodayPhoto,
                $"{data.FullName} - Convert Today Photo - {ImageNameExtension.RemoveSlashesAndColons(data.TimeStamp.ToString())}"
            );
            var imageTwoReference = await imageHelper.DownloadImageAsync(
                data.BaptismPhoto,
                $"{data.FullName} - Baptism Photo - {ImageNameExtension.RemoveSlashesAndColons(data.TimeStamp.ToString())}"
            );

            // Use Placeholder images if no uploaded images were found
            imageOneReference ??= imageSelectionService.GetNextImageReference(ImageSubject.ConversionToday);
            imageTwoReference ??= imageSelectionService.GetNextImageReference(ImageSubject.ConversionBaptism);

            // Ensure no paragraph spacing in stories
            data.ConversionStory = Regex.Replace(data.ConversionStory ?? "", @"\s+", " ").Trim();

            // Replace all html template placeholder data with REAL data from converts
            convertHtml = convertHtml.Replace("{{imageOneReference}}", imageOneReference);
            convertHtml = convertHtml.Replace("{{imageTwoReference}}", imageTwoReference);
            convertHtml = convertHtml.Replace("{{FullName}}", data.FullName);
            convertHtml = convertHtml.Replace("{{Location}}", data.BaptismLocation);
            convertHtml = convertHtml.Replace("{{Year}}", data.BaptismYear.ToString());
            convertHtml = convertHtml.Replace("{{ConversionStory}}", data.ConversionStory);
            convertHtml = convertHtml.Replace(
                "{{ConversionStoryStyle}}",
                FontSizeService.GetFittedFontSizeStyle(data.ConversionStory, 7.38f, 8.2f));


            // Append the new html template to the convert section of booklet
            masterConvertHtml += convertHtml;
        }
        return masterConvertHtml;
    }







}