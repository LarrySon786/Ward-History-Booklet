// This service will perform operations inside google drive
using Google.Apis.Drive.v3;
using Google.Apis.Download;
using DriveFile = Google.Apis.Drive.v3.Data.File;

namespace WoosterHistoryBooklet.Services;


public class GoogleDriveFileService
{
    private readonly DriveService _driveService;

    public GoogleDriveFileService(DriveService driveService)
    {
        _driveService = driveService;
    }

    // Download a Google Drive file to local storage.
    public async Task DownloadFileToPathAsync(string fileId, string destinationPath)
    {
        // Create Directory if not already created
        string? directory = Path.GetDirectoryName(destinationPath);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        string temporaryPath = destinationPath + ".download";

        try
        {
            var request = _driveService.Files.Get(fileId);

            await using (var stream = new FileStream(temporaryPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var progress = await request.DownloadAsync(stream);

                if (progress.Status != DownloadStatus.Completed)
                {
                    throw new IOException($"Google Drive download failed: {progress.Exception}");
                }
            }

            var fileInfo = new FileInfo(temporaryPath);

            if (!fileInfo.Exists || fileInfo.Length == 0)
                throw new IOException("Downloaded file is empty.");

            File.Move(temporaryPath, destinationPath, true);
        }
        catch
        {
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);
            throw;
        }
    }

    // Permanently delete a Google Drive file.
    public async Task DeleteFileAsync(string fileId)
    {
        await _driveService.Files
            .Delete(fileId)
            .ExecuteAsync();
    }
}


