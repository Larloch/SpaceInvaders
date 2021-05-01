using SpaceInvaders.Scripts.Scores;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceInvaders.Scripts.Invasion
{
    public class HeaderManager : MonoBehaviour
    {

        /// <summary>
        ///     ScoreManager is a singleton.
        /// </summary>
        public static HeaderManager Instance { get; private set; }

        /// <summary>
        ///     The name of the player.
        /// </summary>
        [SerializeField] private TMP_Text playerName;

        /// <summary>
        ///     The score of this game in text format.
        /// </summary>
        [SerializeField] private TMP_Text currentScore;

        /// <summary>
        ///     The high-score of this sesssion in text format.
        /// </summary>
        [SerializeField] private TMP_Text highScore;

        void Awake()
        {
            Assert.IsNull(Instance, "Only one instance of HeaderManager is allowed");
            Instance = this;
        }

        void Start()
        {
            playerName.text = ScoreManager.Instance.PlayerName;
            currentScore.text = ScoreManager.Instance.CurrentScore.ToString();
            highScore.text = ScoreManager.Instance.HighScore.ToString();
        }

        public void AddPoints(int points)
        {
            ScoreManager.Instance.CurrentScore = ScoreManager.Instance.CurrentScore + points;
            currentScore.text = ScoreManager.Instance.CurrentScore.ToString();
            if (ScoreManager.Instance.CurrentScore > ScoreManager.Instance.HighScore)
            {
                ScoreManager.Instance.HighScore = ScoreManager.Instance.CurrentScore;
                highScore.text = ScoreManager.Instance.HighScore.ToString();
            }
        }

    }
}