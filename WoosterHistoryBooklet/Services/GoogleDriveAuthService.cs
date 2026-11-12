using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace WoosterHistoryBooklet.Services;

public class GoogleDriveAuthService
{
    public async Task<DriveService> AuthorizeAsync()
    {
        using var stream = new FileStream(
            "credentials.json",
            FileMode.Open,
            FileAccess.Read);

        var secrets = GoogleClientSecrets
            .FromStream(stream)
            .Secrets;

        var credential = await GoogleWebAuthorizationBroker
            .AuthorizeAsync(
                secrets,
                new[] { DriveService.Scope.Drive },
                "user",
                CancellationToken.None,
                new FileDataStore("GoogleDriveTokenStore", true));

        var driveService = new DriveService(
            new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Ward History Booklet"
            });

        return driveService;
    }
}