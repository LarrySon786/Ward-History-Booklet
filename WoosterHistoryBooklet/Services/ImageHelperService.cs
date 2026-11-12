
namespace WoosterHistoryBooklet.Services;

public class ImageHelperService
{
    private readonly GoogleDriveFileService _driveFileService;
    private readonly HashSet<string> _processedFileIds;

    public ImageHelperService(GoogleDriveFileService driveFileService)
    {
        _driveFileService = driveFileService;
        _processedFileIds = new();
    }

    public async Task<string?> DownloadImageAsync(string? driveUrl, string fileName)
    {
        if (string.IsNullOrWhiteSpace(driveUrl)) // return if no image url shared
            return null;

        string fileId = ExtractFileId(driveUrl); // Get the image's file Id
        var filePath = CreateFilePath(fileName); // Create directory and get file path name

        // Avoid downloading an image that already exists locally. Returns file path reference if already existing
        if (File.Exists(filePath))
            return new Uri(filePath).AbsoluteUri;

        // Download using authenticated Google Drive API.
        await _driveFileService.DownloadFileToPathAsync(fileId, filePath);

        // Confirm the local file exists and contains data.
        var fileInfo = new FileInfo(filePath);
        if (!fileInfo.Exists || fileInfo.Length == 0)
            throw new IOException($"Image download verification failed: {fileName}");

        // Only delete the Google Drive original after successfully saving the local copy.
        await _driveFileService.DeleteFileAsync(fileId);

        return new Uri(filePath).AbsoluteUri; // Return image file reference
    }

    private static string ExtractFileId(string url)
    {
        var uri = new Uri(url);

        // Format: drive.google.com/open?id=FILE_ID
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        string? fileId = query["id"];

        if (!string.IsNullOrWhiteSpace(fileId))
            return fileId;

        // Format: drive.google.com/file/d/FILE_ID/view
        string[] segments = uri.AbsolutePath.Split(
            '/',
            StringSplitOptions.RemoveEmptyEntries);

        int fileIndex = Array.IndexOf(segments, "d");

        if (fileIndex >= 0 && fileIndex + 1 < segments.Length)
            return segments[fileIndex + 1];

        throw new ArgumentException(
            "Unable to extract Google Drive file ID.");
    }

    private static string CreateFilePath(string fileName)
    {
        string directory = Path.GetFullPath(Path.Combine("Images", "DataPhotos")); // Prepare to create directory
        Directory.CreateDirectory(directory); // Create directory if not created
        string filePath = Path.GetFullPath(Path.Combine(directory, fileName)); // Create the file path name with image name
        return filePath;
    }
}
