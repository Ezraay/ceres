using Ceres.Core.BattleSystem;

namespace Ceres.Server.Services;

// For testing purposes only, loads a testing deck from CSV
public class CardDeckLoader
{
    public IDeck? Deck { get; private set; }
    private string csvPathFile = Path.Combine(@"Resources/Testing Deck.csv");

    public CardDeckLoader()
    {
        Load(csvPathFile);
    }

    private void Load(string path)
    {
        try
        {
            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var sr = new StreamReader(fs))
            {
                string csvFile = sr.ReadToEnd();
                Deck = new CSVDeck(CardDatabaseLoader.Database, csvFile.Trim(), true);
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("The card deck could not be read:");
            Console.WriteLine(e.Message);
            throw;
        }
    }
}