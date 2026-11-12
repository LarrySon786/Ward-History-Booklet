
using SkiaSharp;

namespace WoosterHistoryBooklet.Services;

public static class FontSizeService
{
    public record FontFitResult(
        float FontSize,
        int LineCount,
        float EstimatedHeight,
        bool Fits
    );

    public static FontFitResult GetFontSize(
        string? text,
        float containerWidthInches,
        float containerHeightInches,
        float maxFontSize = 12f,
        float minFontSize = 9f,
        float step = 0.1f,
        float lineHeight = 1.4f,
        float safetyMargin = 0.05f,
        string fontFamily = "Arial")
    {
        if (containerWidthInches <= 0 || containerHeightInches <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(containerWidthInches));

        if (minFontSize <= 0 ||
            maxFontSize < minFontSize ||
            step <= 0 ||
            lineHeight <= 0 ||
            safetyMargin < 0 ||
            safetyMargin >= 1)
            throw new ArgumentException(
                "Invalid font-fitting configuration.");

        // Convert inches to points (72 points per inch).
        float availableWidth = containerWidthInches * 72f;

        float availableHeight = containerHeightInches * 72f * (1f - safetyMargin);

        using var typeface = SKTypeface.FromFamilyName(fontFamily) ?? SKTypeface.Default;

        string content = text ?? string.Empty;

        FontFitResult? lastResult = null;

        for (float size = maxFontSize; size >= minFontSize - 0.001f; size -= step)
        {
            float fontSize = Math.Max(minFontSize, size);

            using var font = new SKFont(typeface, fontSize);

            int lines = CountLines( content, font, availableWidth);

            float estimatedHeight = lines * fontSize * lineHeight;

            bool fits = estimatedHeight <= availableHeight;

            var result = new FontFitResult(
                fontSize,
                lines,
                estimatedHeight,
                fits
            );

            if (fits)
                return result;

            lastResult = result;
        }

        // Ensure the minimum is tested even when the
        // step does not land exactly on minFontSize.
        if (lastResult == null || lastResult.FontSize > minFontSize)
        {
            using var font = new SKFont(typeface, minFontSize);

            int lines = CountLines(content, font, availableWidth);

            float height = lines * minFontSize * lineHeight;

            lastResult = new FontFitResult(
                minFontSize,
                lines,
                height,
                height <= availableHeight
            );
        }

        return lastResult;
    }

    public static string GetFittedFontSizeStyle(
        string? text,
        float containerWidthInches,
        float containerHeightInches)
    {
        var result = GetFontSize(
            text,
            containerWidthInches,
            containerHeightInches,
            maxFontSize: 12f,
            minFontSize: 9f,
            step: 0.1f,
            lineHeight: 1.5f);

        return FormattableString.Invariant($"font-size:{result.FontSize:0.##}pt;");
    }

    private static int CountLines(
        string text,
        SKFont font,
        float availableWidth)
    {
        if (string.IsNullOrEmpty(text))
            return 0;

        int totalLines = 0;

        // Preserve explicit line breaks.
        string normalized = text.Replace("\r\n", "\n")
                                .Replace('\r', '\n');

        foreach (string paragraph in normalized.Split('\n'))
        {
            if (string.IsNullOrWhiteSpace(paragraph))
            {
                totalLines++;
                continue;
            }

            string[] words = paragraph.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);

            float currentLineWidth = 0;
            float spaceWidth = font.MeasureText(" ");

            totalLines++;

            foreach (string word in words)
            {
                float wordWidth = font.MeasureText(word);

                // Handle words wider than the container.
                if (wordWidth > availableWidth)
                {
                    foreach (char character in word)
                    {
                        float charWidth =
                            font.MeasureText(character.ToString());

                        if (currentLineWidth > 0 &&
                            currentLineWidth + charWidth > availableWidth)
                        {
                            totalLines++;
                            currentLineWidth = 0;
                        }

                        currentLineWidth += charWidth;
                    }

                    continue;
                }

                float requiredWidth = currentLineWidth == 0
                    ? wordWidth
                    : spaceWidth + wordWidth;

                if (currentLineWidth + requiredWidth > availableWidth)
                {
                    totalLines++;
                    currentLineWidth = wordWidth;
                }
                else
                {
                    currentLineWidth += requiredWidth;
                }
            }
        }

        return totalLines;
    }
}
