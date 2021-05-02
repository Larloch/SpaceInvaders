using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceInvaders.Scripts.Scores
{
    /// <summary>
    ///     Class used to handle the serialization and deserialization of the leaderboards.
    ///     It saves a binary file in the playerprefs with current better players and top scores.
    ///     A player can appear in the list multiple times with different scores.
    /// </summary>
    public class LeaderboardsSaver
    {
        /// <summary>
        ///     The filename of the binary serialized file saved in the playerprefs.
        /// </summary>
        private const string SAVED_FILE_NAME = "SpaceInvaders.bytes";

        /// <summary>
        ///     The maximum number of elements saved in the leaderboard.
        ///     It the list has already MAX_SIZE number of elements:
        ///     1) Any new score below the lower one in the list will not be saved.
        ///     2) Including a new score better than the lower one, will remove the lower one from the list.
        /// </summary>
        private const int MAX_SIZE = 10;

        /// <summary>
        ///     SortedList of <(ScoreOwner, Score), Score> used to keep
        ///     updated the leaderboards during the games.
        /// </summary>
        private readonly SortedList<(string, int), int> leaderboardList;

        /// <summary>
        ///     Constructor that initialize the sortedlist used to keep in memory the current leaderboard.
        /// </summary>
        public LeaderboardsSaver()
        {
            leaderboardList = new SortedList<(string, int), int>(MAX_SIZE + 1, new Item2Comparer());
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

        /// <summary>
        ///     Get the best player in the leaderboard.
        ///     If the list is empty, the string of the playerName returned will be null.
        /// </summary>
        /// <returns>A tuple with (playerName, score) of the best player.</returns>
        public (string, int) GetHighScore()
        {
            Assert.IsNotNull(leaderboardList, "The leaderboards should have already been loaded.");
            if (leaderboardList.Count == 0)
            {
                return (null, 0);
            }
            return leaderboardList.Keys[0];
        }

        /// <summary>
        ///     Get all the leaderboard as a List of (playerName, int) tuples.
        /// </summary>
        /// <returns>List of (playerName, int) tuples.</returns>
        public List<(string, int)> GetCompleteLeaderboards()
        {
            Assert.IsNotNull(leaderboardList, "The leaderboards should have already been loaded.");
            return leaderboardList.Keys.ToList();
        }

        /// <summary>
        ///     Save the new score in the leaderboard (if it good enough to be in it).
        ///     If the tuple (owner, score) is already il the leaderboard, the score will be discarded.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="score"></param>
        public void SaveScore(string owner, int score)
        {
            Assert.IsNotNull(leaderboardList, "The leaderboards should have already been loaded.");
            if (leaderboardList.ContainsKey((owner, score)))
            {
                return;
            }
            leaderboardList.Add((owner, score), score);
            if (leaderboardList.Count == MAX_SIZE + 1)
            {
                leaderboardList.RemoveAt(leaderboardList.Count - 1);
            }
            LeaderboardsStructure leaderboards = new LeaderboardsStructure();
            leaderboards.ScoreOwners = leaderboardList.Select(x => x.Key.Item1).ToList();
            leaderboards.Scores = leaderboardList.Values.ToList();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream leeaderboardsFile = File.Create(Path.Combine(Application.persistentDataPath, SAVED_FILE_NAME));
            bf.Serialize(leeaderboardsFile, leaderboards);
            leeaderboardsFile.Close();
        }

        /// <summary>
        ///     Class that implement the IComparer used to sort the leaderboard list.
        ///     Handle the cases when two different player has the same score.
        /// </summary>
        public class Item2Comparer : IComparer<(string, int)>
        {
            int IComparer<(string, int)>.Compare((string, int) x, (string, int) y)
            {
                int result = y.Item2.CompareTo(x.Item2) * MAX_SIZE;
                if (result == 0 && !y.Item1.Equals(x.Item1))
                {
                    result = 1;
                }
                return result;
            }
        }

        /// <summary>
        ///     Serialization class used to save into binary format the leaderboard.
        /// </summary>
        [Serializable]
        public class LeaderboardsStructure
        {
            /// <summary>
            ///     Ordered list with the playerNames (ordered according to the scores).
            /// </summary>
            public List<string> ScoreOwners;

            /// <summary>
            ///     Ordered list of scores (from the highest to the lowest).
            /// </summary>
            public List<int> Scores;
        }
    }
}
