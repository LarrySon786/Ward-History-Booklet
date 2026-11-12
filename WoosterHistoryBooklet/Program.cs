using System.Diagnostics;
using PeachPDF;
using WoosterHistoryBooklet.Extensions;
using WoosterHistoryBooklet.Models;
using WoosterHistoryBooklet.PageGenerators;
using WoosterHistoryBooklet.Services;


// INSTRUCTIONS
// 1) Download all newest csv files from "Google Spreadsheets" (the spreasheet where google forms data ends up)
// 2) Add newest files to "/WardData" directory. These files should ALL be renamed "Responses.csv". One file per folder
// 3) Be sure to delete / move older files (Data should be saved in google sheets. So deletion is fine)



// PRE-REQUISITS  | Login to Google Drive
Console.WriteLine("Attempting to Login to Google Drive...");

var authorizationService = new GoogleDriveAuthService();
var driveService = await authorizationService.AuthorizeAsync();

Console.WriteLine("Google Drive authorization successful!");





// PRE-REQUISITS | Register Services
Console.WriteLine("Setting up Services and Tools...");

var driveFileService = new GoogleDriveFileService(driveService);
var imageHelper = new ImageHelperService(driveFileService);
var imageSelectionService = new ImageSelectionService();

Console.WriteLine("Service and Tool set-up complete!");
Console.WriteLine("Initiating Booklet Generation");
Console.WriteLine("******************************");



// SECTION I | Preassemble Data
// This section reads and assigns all csv data to their proper section

var familyData = CsvExtension.ReadCsv<FamilyDataModel>("WardData/Families/TestResponses.csv");
var missionData = CsvExtension.ReadCsv<MissionDataModel>("WardData/Missions/TestResponses.csv");
// var individualData = CsvExtension.ReadCsv<MissionDataModel>("WardData/Individuals/Responses.csv");
// var wardHistoryData = CsvExtension.ReadCsv<MissionDataModel>("WardData/WardHistory/Responses.csv");
var convertData = CsvExtension.ReadCsv<ConvertsDataModel>("WardData/Converts/TestResponses.csv");

Console.WriteLine("Data Successfully Read");








// SECTION II | Create sections of the booklet - includes table of contents, missions, families, ect
string html = await File.ReadAllTextAsync("HtmlTemplates/Booklet.html"); // Parent HTML container

var masterFamilyHtml = await FamilyPageGenerator.GeneratePages(familyData, imageHelper, imageSelectionService); // Families
var masterMissionHtml = await MissionPageGenerator.GeneratePages(missionData, imageHelper, imageSelectionService); // Missions
var masterConvertsHtml = await ConversionsPageGenerator.GeneratePages(convertData, imageHelper, imageSelectionService); // Converts



// SECTION III | Assemble Booklet | Append all completed sections to the booklet
string bookletContents = masterFamilyHtml + masterMissionHtml + masterConvertsHtml;
html = html.Replace("{{BookletContents}}", bookletContents);

Console.WriteLine("Booklet Assembly Completed");






// SECTION IV | generate pdf file | Create the PDF from the parent html template with all data already inserted
PdfGenerateConfig pdfConfig = new()
    {
        PageSize = PageSize.Letter,
        PageOrientation = PageOrientation.Portrait
    };

    PdfGenerator generator = new();

    var stream = new MemoryStream();
    
    // Load the global stylesheet before template-specific styles.
    string css = await File.ReadAllTextAsync("HtmlTemplates/Global.css")
        + await File.ReadAllTextAsync("HtmlTemplates/Family.css")
        + await File.ReadAllTextAsync("HtmlTemplates/Missionary.css")
        + await File.ReadAllTextAsync("HtmlTemplates/Convert.css");
    var cssData = await generator.ParseStyleSheet(css);

    var document = await generator.GeneratePdf(html, pdfConfig, cssData);
    document.Save(stream);

Console.WriteLine("Booklet PDF Generation Completed. Now saving...");





// SECTION V | save pdf to file. Check "Output" folder for generated PDFs.
Directory.CreateDirectory("Output");
    string path = "Output/WardHistoryBooklet.pdf";
    await File.WriteAllBytesAsync(path, stream.ToArray());

Console.WriteLine($"PDF successfully saved to: {Path.GetFullPath(path)}");
Process.Start(new ProcessStartInfo
{
    FileName = Path.GetFullPath(path),
    UseShellExecute = true
});



// Completed Processing
Console.WriteLine("******************************");
Console.WriteLine("Process Completed");