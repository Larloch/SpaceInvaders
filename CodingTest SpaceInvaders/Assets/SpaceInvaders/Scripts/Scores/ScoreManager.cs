using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceInvaders.Scripts.Scores
{
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

        public void SaveFinalScore()
        {
            leaderboards.SaveScore(PlayerName, CurrentScore);
        }
    }
}