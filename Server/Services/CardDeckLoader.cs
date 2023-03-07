using System.Reflection;
using Ceres.Core.BattleSystem;

namespace Ceres.Server.Services;

// For testing purposes only, loads a testing deck from CSV
public class CardDeckLoader
{
    public IDeck? Deck { get; private set; }
    private string csvPathFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data/Testing Deck.csv");

    public CardDeckLoader(CardDatabaseLoader cardDatabaseLoader)
    {
        Load(cardDatabaseLoader.Database, csvPathFile);
    }

    private void Load(ICardDatabase database, string path)
    {
        try
        {
            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var sr = new StreamReader(fs))
            {
                string csvFile = sr.ReadToEnd();
                Deck = new CSVDeck(database, csvFile.Trim(), true);
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