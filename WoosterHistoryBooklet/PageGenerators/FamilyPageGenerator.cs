
using System.Text.RegularExpressions;
using PeachDrawing.Core;
using WoosterHistoryBooklet.Extensions;
using WoosterHistoryBooklet.Models;
using WoosterHistoryBooklet.Services;

namespace WoosterHistoryBooklet.PageGenerators;

public static class FamilyPageGenerator
{
    public static async Task<string> GeneratePages(
        List<FamilyDataModel> familyData,
        ImageHelperService imageHelper,
        ImageSelectionService imageSelectionService)
    {
        // Create the entire html section for family
        string masterFamilyHtml = string.Empty;

        foreach (var data in familyData)
        {
            // Create a new individual family section for each family | Load HTML template
            string familyHtml = await File.ReadAllTextAsync("HtmlTemplates/Family.html");

            // Download Images from Google Drive if not already downloaded:
            var imageOneReference = await imageHelper.DownloadImageAsync(
                data.FamilyPhotograph,
                $"{data.FamilySurname} - Family Photo - {ImageNameExtension.RemoveSlashesAndColons(data.TimeStamp.ToString())}"
            );

            var imageTwoReference = await imageHelper.DownloadImageAsync(
                data.WeddingOrAlternatePhoto,
                $"{data.FamilySurname} - Parent Photo - {ImageNameExtension.RemoveSlashesAndColons(data.TimeStamp.ToString())}"
            );

            var imageThreeReference = await imageHelper.DownloadImageAsync(
                data.TemplePhotograph,
                $"{data.FamilySurname} - Temple Photo - {ImageNameExtension.RemoveSlashesAndColons(data.TimeStamp.ToString())}"
            );

            // Use placeholder images if no existing images uploaded
            imageOneReference ??= imageSelectionService.GetNextImageReference(ImageSubject.FamilyPhoto);
            imageTwoReference ??= imageSelectionService.GetNextImageReference(ImageSubject.WeddingPhotoOrAlternate);
            imageThreeReference ??= imageSelectionService.GetNextImageReference(ImageSubject.TempleFiller);

            // Ensure no paragraph spacing in stories / Bios
            data.FamilyBiography = Regex.Replace(data.FamilyBiography ?? "", @"\s+", " ").Trim();
            data.MotherBiography = Regex.Replace(data.MotherBiography ?? "", @"\s+", " ").Trim();
            data.FatherBiography = Regex.Replace(data.FatherBiography ?? "", @"\s+", " ").Trim();
            data.FamilyStory = Regex.Replace(data.FamilyStory ?? "", @"\s+", " ").Trim();
            data.TempleMeaning = Regex.Replace(data.TempleMeaning ?? "", @"\s+", " ").Trim();

            // Replace all page one and two html template placeholder data with REAL data from Families
            familyHtml = familyHtml.Replace("{{FamilyPhotoReference}}", imageOneReference);
            familyHtml = familyHtml.Replace("{{DatesMarkUp}}", CreateFamilyWardDates(data));
            familyHtml = familyHtml.Replace("{{FamilyBiography}}", data.FamilyBiography);
            familyHtml = familyHtml.Replace("{{FamilyBiographyStyle}}", FontSizeService.GetFittedFontSizeStyle(data.FamilyBiography, 7.38f, 2.2f));
            familyHtml = familyHtml.Replace("{{WeddingPhotoReference}}", imageTwoReference);
            familyHtml = familyHtml.Replace("{{FatherMarkup}}", CreateFatherMarkup(data));
            familyHtml = familyHtml.Replace("{{MotherMarkup}}", CreateMotherMarkup(data));
            familyHtml = familyHtml.Replace("{{ChildrenMarkup}}", CreateChildrenMarkup(data));

            // Create pages three and four
            string pageThreeAndFour = CreatePageThreeAndFourMarkUp(data, imageThreeReference);

            // Insert pages three and four into the HTML template
            familyHtml = familyHtml.Replace("{{PageThreeAndFour}}", pageThreeAndFour);
            familyHtml = familyHtml.Replace("{{FamilySurname}}", data.FamilySurname);

            // Append the new html template to the family section of booklet
            masterFamilyHtml += familyHtml;
        }

        return masterFamilyHtml;
    }


    // Create Ward Dates (Year Arrived - Year Left)
    private static string CreateFamilyWardDates(FamilyDataModel data)
    {
        string datesMarkUp = string.IsNullOrWhiteSpace(data.YearLeftWard.ToString())
            ? $"Wooster Ward: {data.YearJoinedWard}"
            : $"Wooster Ward: {data.YearJoinedWard} - {data.YearLeftWard}";

        return datesMarkUp;
    }


    // FATHER BIOGRAPHY MARKUP
    private static string CreateFatherMarkup(FamilyDataModel data)
    {
        string fatherMarkUp = string.Empty;

        if (!string.IsNullOrWhiteSpace(data.FatherFullName) &&
            !string.IsNullOrWhiteSpace(data.FatherBiography))
        {
            fatherMarkUp =
            $"""
                <div class="parent-bio">
                    <h3>{data.FatherFullName}</h3>
                    <p class="body-copy" style="{FontSizeService.GetFittedFontSizeStyle(data.FatherBiography, 5f, 1.1f)}">{data.FatherBiography}</p>
                </div>
            """;
        }
        else if (string.IsNullOrWhiteSpace(data.FatherFullName) &&
                 !string.IsNullOrWhiteSpace(data.FatherBiography))
        {
            fatherMarkUp =
            $"""
                <div class="parent-bio">
                    <h3>{data.FamilySurname} Dad</h3>
                    <p class="body-copy" style="{FontSizeService.GetFittedFontSizeStyle(data.FatherBiography, 5f, 1.1f)}">{data.FatherBiography}</p>
                </div>
            """;
        }
        else if (!string.IsNullOrWhiteSpace(data.FatherFullName) &&
                 string.IsNullOrWhiteSpace(data.FatherBiography))
        {
            fatherMarkUp =
            $"""
                <div class="parent-bio">
                    <h3>{data.FatherFullName}</h3>
                    <p class="body-copy" style="{FontSizeService.GetFittedFontSizeStyle($"The father of the {data.FamilySurname} home.", 5f, 1.1f)}">The father of the {data.FamilySurname} home.</p>
                </div>
            """;
        }

        return fatherMarkUp;
    }


    // MOTHER BIOGRAPHY MARKUP
    private static string CreateMotherMarkup(FamilyDataModel data)
    {
        string motherMarkUp = string.Empty;

        if (!string.IsNullOrWhiteSpace(data.MotherFullName) &&
            !string.IsNullOrWhiteSpace(data.MotherBiography))
        {
            motherMarkUp =
            $"""
                <div class="parent-bio">
                    <h3>{data.MotherFullName}</h3>
                    <p class="body-copy" style="{FontSizeService.GetFittedFontSizeStyle(data.MotherBiography, 5f, 1.1f)}">{data.MotherBiography}</p>
                </div>
            """;
        }
        else if (string.IsNullOrWhiteSpace(data.MotherFullName) &&
                 !string.IsNullOrWhiteSpace(data.MotherBiography))
        {
            motherMarkUp =
            $"""
                <div class="parent-bio">
                    <h3>{data.FamilySurname} Mom</h3>
                    <p class="body-copy" style="{FontSizeService.GetFittedFontSizeStyle(data.MotherBiography, 5f, 1.1f)}">{data.MotherBiography}</p>
                </div>
            """;
        }
        else if (!string.IsNullOrWhiteSpace(data.MotherFullName) &&
                 string.IsNullOrWhiteSpace(data.MotherBiography))
        {
            motherMarkUp =
            $"""
                <div class="parent-bio">
                    <h3>{data.MotherFullName}</h3>
                    <p class="body-copy" style="{FontSizeService.GetFittedFontSizeStyle($"The mother of the {data.FamilySurname} home.", 5f, 1.1f)}">The mother of the {data.FamilySurname} home.</p>
                </div>
            """;
        }

        return motherMarkUp;
    }


    // CHILDREN MARKUP
    private static string CreateChildrenMarkup(FamilyDataModel data)
    {
        string childrenMarkUp = string.Empty;

        string childContainer =
        """
            <section class="parent-bio children-section">
                <p class="children-header">Children</p>
                <ul style="{{ChildrenStyle}}">
                    {{Children}}
                </ul>
            </section>
        """;


        string allChildren = string.Empty;
        string individualChild = string.Empty;

        foreach (var child in data.Children)
        {
            individualChild =
            $"""
                <li>{child}</li>
            """;

            allChildren = allChildren + individualChild;
        }

        if (!string.IsNullOrWhiteSpace(allChildren))
        {
            childContainer = childContainer.Replace("{{Children}}", allChildren);
            childContainer = childContainer.Replace(
                "{{ChildrenStyle}}",
                FontSizeService.GetFittedFontSizeStyle(string.Join("\n", data.Children), 1.4f, 4.1f));
            childrenMarkUp = childContainer;
        }

        return childrenMarkUp;
    }


    // FAMILY STORY MARKUP
    private static string CreateFamilyStoryMarkup(FamilyDataModel data)
    {
        string familyStoryMarkup = string.Empty;

        if (!string.IsNullOrWhiteSpace(data.FamilyStory))
        {
            familyStoryMarkup =
            $"""
                <h2>{data.StoryTitle ?? "{{FamilySurname}} Story"}</h2>

                <div class="body-copy" style="{FontSizeService.GetFittedFontSizeStyle(data.FamilyStory, 7.38f, 8.3f)}">
                    {data.FamilyStory}
                </div>
            """;
        }
        else
        {
            familyStoryMarkup = "";
        }

        return familyStoryMarkup;
    }


    // TEMPLE INFORMATION MARKUP
    private static string CreateTempleInformationMarkup(FamilyDataModel data)
    {
        string templeInformationMarkup = string.Empty;

        if (!string.IsNullOrWhiteSpace(data.Temple)
            && !string.IsNullOrWhiteSpace(data.TempleMeaning)
            && !string.IsNullOrEmpty(data.TemplePhotograph))
        {
            templeInformationMarkup =
            $"""
                <div class="temple-information">
                    <h2 class="text-align: center;">{data.Temple}</h2>

                    <div class="body-copy remove-indent" style="{FontSizeService.GetFittedFontSizeStyle(data.TempleMeaning, 7.38f, 2.4f)};">
                        {data.TempleMeaning}
                    </div>
                </div>
            """;
        }

        return templeInformationMarkup;
    }


    // PAGE THREE AND FOUR CONTAINER MARKUP
    private static string CreatePageThreeAndFourMarkUp(FamilyDataModel data, string templePhotographReference)
    {
        string pageThreeAndFourMarkup = string.Empty;

        pageThreeAndFourMarkup += GeneratePageThree(data);
        pageThreeAndFourMarkup += GeneratePageFour(data, templePhotographReference);

        return pageThreeAndFourMarkup;
    }

    private static string GeneratePageThree(FamilyDataModel data)
    {
        // PAGE THREE
        string pageThree = string.Empty;
        string familyStoryMarkup = CreateFamilyStoryMarkup(data);

        // If Family Story is written, then generate family story markup no matter what.
        if (!string.IsNullOrWhiteSpace(familyStoryMarkup))
        {
            pageThree +=
            $"""
            <!-- PAGE 3: FAMILY STORY -->
            <section class="page page-three">
                <header class="family-header">
                    <h1>Families and Individuals</h1>
                </header>

                <section class="page-content">
                    <section class="family-story">
                        {familyStoryMarkup}
                    </section>
                </section>
                <footer class="page-footer">
                    <span>Ward History - {data.FamilySurname} Family</span>
                    <span>3</span>
                </footer>
            </section>
            """;
        }
        // If family story is NOT written and the user has uploaded all temple data, then generate a placeholder image.
        else if (string.IsNullOrWhiteSpace(data.FamilyStory)
            && !string.IsNullOrWhiteSpace(data.Temple)
            && !string.IsNullOrWhiteSpace(data.TemplePhotograph)
            && !string.IsNullOrWhiteSpace(data.TempleMeaning))
        {
            pageThree +=
            $"""
            <!-- PAGE 3: FAMILY STORY -->
            <section class="page page-three">
                <header class="family-header">
                    <h1>Families and Individuals</h1>
                </header>
                <section class="page-content" style="justify-content: center;">
                    <div class="ordinary-photo-container">
                        <img src="Images/PlaceholderImages/MissionaryProfile/MissionaryProfileTwo.jpg" alt="Placeholder photo of gospel principles">
                    </div>
                </section>
                <footer class="page-footer">
                    <span>Ward History - {data.FamilySurname} Family</span>
                    <span>3</span>
                </footer>
            </section>
            """;
        }
        return pageThree;

    }

    private static string GeneratePageFour(FamilyDataModel data, string templePhotographReference)
    {
        string pageFour = string.Empty;
        string templeInformationMarkup = CreateTempleInformationMarkup(data);
        string templeContainerStyle = string.Empty;
        string templePhotoContainerStyle = string.Empty;
        string templePhotoStyle = string.Empty;

        // PAGE FOUR
        if (!string.IsNullOrWhiteSpace(data.TempleMeaning)
            && !string.IsNullOrWhiteSpace(data.StoryTitle)
            && !string.IsNullOrWhiteSpace(data.TemplePhotograph))
        {

            pageFour +=
            $"""
            <!-- PAGE 4: FAMILY TEMPLE -->
            <section class="page page-four">
                <header class="family-header">
                    <h1>Families and Individuals</h1>
                </header>

                <section class="page-content">
                    <section class="temple-section">

                        <div class="ordinary-photo-container temple-photo-container" style="width:100%; height:auto;">
                            <img src="{templePhotographReference}" alt="Photograph of {data.Temple}">
                        </div>

                        {templeInformationMarkup}

                    </section>
                </section>

                <footer class="page-footer">
                    <span>Ward History - {data.FamilySurname} Family</span>
                    <span>4</span>
                </footer>
            </section>
            """;
        }

        // If user did not upload all temple data, then ONLY GENERATE page 4 if page 3 was generated. Otherwise leave Page 3 + 4 blank.
        else if (!string.IsNullOrWhiteSpace(data.FamilyStory)
            && (string.IsNullOrWhiteSpace(data.TempleMeaning)
                || string.IsNullOrWhiteSpace(data.StoryTitle)
                || string.IsNullOrWhiteSpace(data.TemplePhotograph)))
        {
            if (string.IsNullOrWhiteSpace(templeInformationMarkup))
            {
                templeContainerStyle = "justify-content: center; align-items: center;";
                templePhotoContainerStyle = "";
                templePhotoStyle = "";
            }

            pageFour +=
            $"""
            <!-- PAGE 4: FAMILY TEMPLE -->
            <section class="page page-four">
                <header class="family-header">
                    <h1>Families and Individuals</h1>
                </header>

                <section class="page-content">
                    <section class="temple-section"  style="{templeContainerStyle}">
                        <div class="ordinary-photo-container temple-photo-container" style="width:100%; height:auto; {templePhotoContainerStyle}">
                            <img src="{templePhotographReference}" alt="Photograph of {data.Temple}  style="${templePhotoStyle}"">
                        </div>
                    </section>
                </section>

                <footer class="page-footer">
                    <span>Ward History - {data.FamilySurname} Family</span>
                    <span>4</span>
                </footer>
            </section>
            """;
        }

        return pageFour;
    }


}
