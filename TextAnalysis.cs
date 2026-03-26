// EF Core 10: complex type — key-less value object, columns live inside UserInputs table
public class TextAnalysis
{
    // Stored as column Analysis_WordCount in the UserInputs table
    public int WordCount { get; set; }

    // Stored as column Analysis_CharacterCount in the UserInputs table
    public int CharacterCount { get; set; }
}
