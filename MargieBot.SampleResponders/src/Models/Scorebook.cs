using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

namespace MargieBot.SampleResponders.Models
{
    public class Scorebook
    {
        private Dictionary<string, int> Scores { get; }
        private string TeamId { get; }

        public Scorebook(string teamId)
        {
            Scores = new Dictionary<string, int>();
            TeamId = teamId;
            var filePath = GetFilePath();

            if (File.Exists(filePath)) {
                Scores = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(filePath));
            }
        }

        private string GetFilePath()
        {
            return TeamId + ".json";
        }

        public IReadOnlyDictionary<string, int> GetScores()
        {
            return new ReadOnlyDictionary<string, int>(Scores);
        }

        public int GetUserScore(string userId)
        {
            return Scores.ContainsKey(userId) ? Scores[userId] : 0;
        }

        public bool HasUserScored(string userId)
        {
            return Scores.ContainsKey(userId);
        }

        public void ScoreUser(string userId, int increment)
        {
            ScoreUsers(new[] { userId }, increment);
        }

        public void ScoreUsers(IEnumerable<string> userIds, int increment)
        {
            foreach (var userId in userIds) {
                if (Scores.ContainsKey(userId)) {
                    Scores[userId] += increment;
                }
                else {
                    Scores.Add(userId, increment);
                }
            }
            Save();
        }

        private void Save()
        {
            // TODO: exception handling, y'all
            File.WriteAllText(GetFilePath(), JsonConvert.SerializeObject(Scores));
        }
    }
}