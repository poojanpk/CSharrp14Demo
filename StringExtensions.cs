// C# 14: new extension member syntax — groups extension properties on a receiver type
public static class StringExtensions
{
    extension(string text)
    {
        // Returns the total number of words, ignoring consecutive spaces
        public int WordCount =>
            text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

        // Returns the count of non-whitespace characters
        public int CharacterCount =>
            text.Replace(" ", string.Empty).Length;

        // True when the string is null, empty, or whitespace only
        public bool IsEmpty =>
            string.IsNullOrWhiteSpace(text);
    }
}
