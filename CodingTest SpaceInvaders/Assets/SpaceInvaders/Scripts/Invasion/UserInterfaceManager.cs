using SpaceInvaders.Scripts.Scores;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceInvaders.Scripts.Invasion
{
    public class UserInterfaceManager : MonoBehaviour
    {

        /// <summary>
        ///     ScoreManager is a singleton.
        /// </summary>
        public static UserInterfaceManager Instance { get; private set; }

        /// <summary>
        ///     The name of the player.
        /// </summary>
        [SerializeField] private TMP_Text playerName;

        /// <summary>
        ///     The score of this game in text format.
        /// </summary>
        [SerializeField] private TMP_Text currentScore;

        /// <summary>
        ///     The name of the player who realized the high-score of this session.
        /// </summary>
        [SerializeField] private TMP_Text highScoreOwner;

        /// <summary>
        ///     The high-score of this sesssion in text format.
        /// </summary>
        [SerializeField] private TMP_Text highScore;

        /// <summary>
        ///     The central panel.
        /// </summary>
        [SerializeField] private GameObject centralPanel;

        /// <summary>
        ///     The central panel title.
        /// </summary>
        [SerializeField] private TMP_Text centralTitle;

        /// <summary>
        ///     The central panel message.
        /// </summary>
        [SerializeField] private TMP_Text centralMessage;


        void Awake()
        {
            Assert.IsNull(Instance, "Only one instance of UserInterfaceManager is allowed");
            Instance = this;
        }

        void Start()
        {
            playerName.text = ScoreManager.Instance.PlayerName;
            if (ScoreManager.Instance.HighScoreOwner != null)
            {
                highScoreOwner.text = ScoreManager.Instance.HighScoreOwner;
            }
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
                highScoreOwner.text = ScoreManager.Instance.PlayerName;
                ScoreManager.Instance.HighScoreOwner = ScoreManager.Instance.PlayerName;
            }
        }

        public void SetLevel(int level)
        {
            centralTitle.text = "LEVEL - " + level;
        }

        public void CloseCentral()
        {
            centralPanel.SetActive(false);
        }

        public void OpenPause()
        {
            centralTitle.text = "PAUSE";
            centralMessage.text = "PRESS: FIRE TO RESUME, QUIT TO LEAVE";
            centralPanel.SetActive(true);
        }
    }
}