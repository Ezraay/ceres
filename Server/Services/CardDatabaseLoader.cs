using System.Reflection;
using Ceres.Core.BattleSystem;

namespace Ceres.Server.Services;

public class CardDatabaseLoader
{
    public ICardDatabase? Database { get; private set; }

    private readonly string csvPathFile =
        Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data/Cards.csv");

    public CardDatabaseLoader()
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
                Database = new CSVCardDatabase(csvFile.Trim());
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("The card database could not be read:");
            Console.WriteLine(e.Message);
            throw;
        }
    }
}