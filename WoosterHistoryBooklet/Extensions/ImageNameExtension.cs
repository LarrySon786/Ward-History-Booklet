// REMOVE slashes in images names (This is for inserting timestamps into the image name)

namespace WoosterHistoryBooklet.Extensions;

public static class ImageNameExtension
{

    public static string RemoveSlashesAndColons(string s)
    {
        string result = s.Replace("/", "-");
        result = result.Replace("'\'", "-");
        result = result.Replace(":", "-");
        return result;
    }
}