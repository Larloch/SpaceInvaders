using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpaceInvaders.Scripts.Scores
{
    /// <summary>
    ///     Singleton class that keeps in memory the player score and the high score (with the relative names).
    ///     This is attached to its gameobject in the MainMenu scene, but it will be not destroyed on load.
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        /// <summary>
        ///     ScoreManager is a singleton.
        /// </summary>
        public static ScoreManager Instance { get; private set; }

        /// <summary>
        ///     Current player.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        ///     Name of the player that own the Highscore of this session.
        /// </summary>
        public string HighScoreOwner { get; set; }

        /// <summary>
        ///     Highscore of this session.
        /// </summary>
        public int HighScore { get; set; }

        /// <summary>
        ///     Current score of this session.
        /// </summary>
        public int CurrentScore { get; set; }

        /// <summary>
        ///     Current level of this game.
        /// </summary>
        public int CurrentLevel { get; set; }

        /// <summary>
        ///     The leaderboard handler.
        /// </summary>
        private LeaderboardsSaver leaderboards;

        /// <summary>
        ///     Attach the reference of this instance for its access as a singleton and create
        ///     the LeaderboardSaver instance to manage the persistance of the hi-scores.
        ///     Also it request from the LeaderboardSaver the current high score.
        /// </summary>
        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            leaderboards = new LeaderboardsSaver();
            (HighScoreOwner, HighScore) = leaderboards.GetHighScore();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        ///     Save the last score of the player in the leaderboard.
        ///     Note that if the score is too low to enter in the leaderboard it will not be saved.
        ///     This is called by the GameOverManager.
        /// </summary>
        public void SaveFinalScore()
        {
            leaderboards.SaveScore(PlayerName, CurrentScore);
        }

        /// <summary>
        ///     Request from the LeaderboardsSaver the top players. 
        /// </summary>
        /// <param name="size">The size of the list returned.</param>
        /// <returns>A list of tuples of (playerName, score) ordere from the highest score to the lower.</returns>
        public List<(string, int)> GetLeaderboardTop(int size)
        {
            return leaderboards.GetCompleteLeaderboards().Take(size).ToList();
        }
    }
}
