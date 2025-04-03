namespace RecommenderProject.API.Services
{
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
            return ReadContentCsv(_contentPath, itemId);
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
                    result.AddRange(parts.Skip(2).Take(5)); // Ensures only 5 recommendations are returned
                    break;
                }
            }

            return result;
        }

        private List<string> ReadContentCsv(string filePath, string id)
        {
            var result = new List<string>();

            if (!File.Exists(filePath))
                return result;

            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            var matrix = new List<List<string>>();

            // Reading the CSV into a matrix for content-based recommendations
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                matrix.Add(parts.ToList());
            }

            // Find the column that matches the itemId
            int targetColumnIndex = -1;
            for (int i = 0; i < matrix[0].Count; i++)
            {
                if (matrix[0][i] == id)
                {
                    targetColumnIndex = i;
                    break;
                }
            }

            if (targetColumnIndex == -1) // If the itemId is not found
                return result;

            // Sort the column based on values in descending order, excluding the first row (itemIds)
            var recommendations = matrix.Skip(2) // Skip the header row
                                .Select(row => new { contentId = row[0], value = row[targetColumnIndex] })
                                .Where(x => decimal.TryParse(x.value, out decimal parsedValue)) // Filter out non-numeric values
                                .OrderByDescending(x => decimal.Parse(x.value)) // Safely parse the value after filtering
                                .Take(5)
                                .Select(x => x.contentId)
                                .ToList();

            return recommendations;
        }
    }
}
