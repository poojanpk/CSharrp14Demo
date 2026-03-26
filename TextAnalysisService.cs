// Service for text analysis — uses C# 14 extension members from StringExtensions.cs
public class TextAnalysisService
{
    // Builds a TextAnalysis value object using C# 14 extension properties on string
    public TextAnalysis Analyze(string text) => new()
    {
        WordCount      = text.WordCount,      // C# 14: extension property on string
        CharacterCount = text.CharacterCount  // C# 14: extension property on string
    };

    // Delegates to the C# 14 IsEmpty extension property for a null/whitespace check
    public bool IsEmpty(string text) => text.IsEmpty;
}
