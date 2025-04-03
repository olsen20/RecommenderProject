namespace RecommenderProject.API.Services;

using System.Text;

public class CSVRecommendations
{
    private readonly string _collabPath = "Data/collaborativeRecommender.csv";
    private readonly string _contentPath = "Data/contentRecommender.csv";

    public List<string> GetCollaborative(string itemId)
    {
        return ReadCsv(_collabPath, itemId);
    }

    public List<string> GetContentBased(string itemId)
    {
        return ReadCsv(_contentPath, itemId);
    }

    private List<string> ReadCsv(string filePath, string id)
    {
        var result = new List<string>();

        if (!File.Exists(filePath))
            return result;

        var lines = File.ReadAllLines(filePath, Encoding.UTF8);
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts[0] == id)
            {
                result.AddRange(parts.Skip(1).Take(5));
                break;
            }
        }

        return result;
    }
}
