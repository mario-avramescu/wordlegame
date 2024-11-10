namespace WordleGame.Results;

internal sealed class ResultWriter(ITextTerminal terminal)
{
    private const string ResultMessage = "> Word guess: ";
    private const string CorrectLetterPosition = "\x1b[92m"; 
    private const string CorrectLetter = "\x1b[93m"; 
    private const string ResetColor = "\x1b[0m";

    private readonly ITextTerminal terminal = terminal;

    public void WriteResult(string guessedWord, string targetWord)
    {
        var letterCount = targetWord.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
        var result = new char[5];

        for (byte i = 0; i < 5; i++)
        {
            result[i] = guessedWord[i] == targetWord[i] ? '2' : '0';
            if (result[i] == '2') letterCount[guessedWord[i]]--;
        }

        for (byte i = 0; i < 5; i++)
        {
            if (result[i] == '0' && letterCount.TryGetValue(guessedWord[i], out int count) && count > 0)
            {
                result[i] = '1';
                letterCount[guessedWord[i]]--;
            }
        }

        string formattedResult = string.Concat(Enumerable.Range(0, 5).Select(i =>
            result[i] switch
            {
                '2' => $"{CorrectLetterPosition}{guessedWord[i]}{ResetColor}",
                '1' => $"{CorrectLetter}{guessedWord[i]}{ResetColor}",
                _ => guessedWord[i].ToString()
            }
        ));

        terminal.WriteText(ResultMessage + formattedResult + "\n");
    }
}
