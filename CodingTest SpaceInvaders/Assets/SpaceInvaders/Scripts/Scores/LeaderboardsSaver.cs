using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceInvaders.Scripts.Scores
{
    public class LeaderboardsSaver
    {
        private const string SAVED_FILE_NAME = "SpaceInvaders.bytes";

        /// <summary>
        ///     SortedList of <(ScoreOwner, Score), Score> used to keep
        ///     updated the leaderboards during the games.
        /// </summary>
        private SortedList<(string, int), int> leaderboardList;

        public LeaderboardsSaver()
        {
            leaderboardList = new SortedList<(string, int), int>(new Item2Comparer());
            string leaderboardsFilePath = Path.Combine(Application.persistentDataPath, SAVED_FILE_NAME);
            if (File.Exists(leaderboardsFilePath))
            {
                FileStream leeaderboardsFile = File.OpenRead(leaderboardsFilePath);
                var bf = new BinaryFormatter();
                LeaderboardsStructure leaderboards = (LeaderboardsStructure)bf.Deserialize(leeaderboardsFile);
                leeaderboardsFile.Close();
                for (int position = 0; position < leaderboards.Scores.Count; ++position)
                {
                    leaderboardList.Add((leaderboards.ScoreOwners[position], leaderboards.Scores[position]), leaderboards.Scores[position]);
                }
            }
        }

        public (string, int) GetHighScore()
        {
            if (leaderboardList.Count == 0)
            {
                return (null, 0);
            }
            return leaderboardList.Keys[0];
        }

        public List<(string, int)> Load()
        {
            Assert.IsNotNull(leaderboardList, "The leaderboards should have already been loaded.");
            return leaderboardList.Keys.ToList();
        }

        public void SaveScore(string owner, int score)
        {
            Assert.IsNotNull(leaderboardList, "The leaderboards should have already been loaded.");
            leaderboardList.Add((owner, score), score);
            LeaderboardsStructure leaderboards = new LeaderboardsStructure();
            leaderboards.ScoreOwners = leaderboardList.Select(x => x.Key.Item1).ToList();
            leaderboards.Scores = leaderboardList.Values.ToList();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream leeaderboardsFile = File.Create(Path.Combine(Application.persistentDataPath, SAVED_FILE_NAME));
            bf.Serialize(leeaderboardsFile, leaderboards);
            leeaderboardsFile.Close();
        }

        public class Item2Comparer : IComparer<(string, int)>
        {
            int IComparer<(string, int)>.Compare((string, int) a, (string, int) b)
            {
                return b.Item2.CompareTo(a.Item2);
            }
        }

        [Serializable]
        public class LeaderboardsStructure
        {
            public List<string> ScoreOwners;

            public List<int> Scores;
        }
    }
}
