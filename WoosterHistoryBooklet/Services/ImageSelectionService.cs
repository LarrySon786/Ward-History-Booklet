using WoosterHistoryBooklet.Models;

namespace WoosterHistoryBooklet.Services;

public class ImageSelectionService
{
    private static readonly HashSet<string> SupportedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".bmp",
        ".gif",
        ".jpeg",
        ".jpg",
        ".png",
        ".webp",
    };

    private readonly Dictionary<ImageSubject, int> _nextImageIndexes = [];
    private readonly object _selectionLock = new();

    public string GetNextImageReference(ImageSubject subject)
    {
        lock (_selectionLock)
        {
            string placeholderRoot = ResolvePlaceholderRootDirectory();
            string? categoryFolder = GetCategoryFolder(subject);
            string searchDirectory = categoryFolder is null
                ? placeholderRoot
                : Path.Combine(placeholderRoot, categoryFolder);

            if (!Directory.Exists(searchDirectory))
            {
                throw new DirectoryNotFoundException(
                    $"The placeholder image directory for '{subject}' was not found: {searchDirectory}");
            }

            string[] images = Directory
                .EnumerateFiles(searchDirectory, "*", SearchOption.AllDirectories)
                .Where(path => SupportedImageExtensions.Contains(Path.GetExtension(path)))
                .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (images.Length == 0)
            {
                throw new FileNotFoundException(
                    $"No supported placeholder images were found for '{subject}' in '{searchDirectory}'.");
            }

            int nextIndex = _nextImageIndexes.GetValueOrDefault(subject);
            string imagePath = images[nextIndex % images.Length];
            _nextImageIndexes[subject] = (nextIndex + 1) % images.Length;

            return new Uri(Path.GetFullPath(imagePath)).AbsoluteUri;
        }
    }

    private static string ResolvePlaceholderRootDirectory()
    {
        foreach (string startPath in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
        {
            DirectoryInfo? directory = new(startPath);
            while (directory is not null)
            {
                string candidate = Path.Combine(directory.FullName, "Images", "PlaceholderImages");
                if (Directory.Exists(candidate))
                {
                    return candidate;
                }

                directory = directory.Parent;
            }
        }

        return Path.GetFullPath(Path.Combine("Images", "PlaceholderImages"));
    }

    private static string? GetCategoryFolder(ImageSubject subject) => subject switch
    {
        ImageSubject.Any => null,
        ImageSubject.FamilyPhoto => "FamilyPhoto",
        ImageSubject.WeddingPhotoOrAlternate => "FamilyWeddingPhoto",
        ImageSubject.TempleFiller => "FamilyTempleFiller",
        ImageSubject.ConversionToday => "ConversionTodayPhoto",
        ImageSubject.ConversionBaptism => "ConversionBaptismPhoto",
        ImageSubject.MissionProfile => "MissionaryProfile",
        ImageSubject.MissionLandscape => "MissionaryLandscape",
        _ => throw new ArgumentOutOfRangeException(nameof(subject), subject, "Unsupported image subject."),
    };
}