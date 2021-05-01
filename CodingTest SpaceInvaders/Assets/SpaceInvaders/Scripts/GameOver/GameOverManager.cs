using SpaceInvaders.Scripts.Scores;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceInvaders.Scripts.GameOver
{
    public class GameOverManager : MonoBehaviour
    {
        /// <summary>
        ///     The name of the player.
        /// </summary>
        [SerializeField] private TMP_Text playerName;

        /// <summary>
        ///     The score of this game in text format.
        /// </summary>
        [SerializeField] private TMP_Text currentScore;

        void Start()
        {
            playerName.text = ScoreManager.Instance.PlayerName;
            currentScore.text = ScoreManager.Instance.CurrentScore.ToString();
        }

        /// <summary>
        ///     Called when the player clicks on the restart button
        /// </summary>
        public void OnRestartButtonClick()
        {
            ScoreManager.Instance.CurrentScore = 0;
            ScoreManager.Instance.CurrentLevel = 1;
            SceneManager.LoadScene("Invasion");
        }

        /// <summary>
        ///     Called when the player clicks on the main menu button
        /// </summary>
        public void OnMainMenuButtonClick()
        {
            SceneManager.LoadScene("MainMenu");
        }

        /// <summary>
        ///     Called when the player clicks on the quit button
        /// </summary>
        public void OnQuitButtonClick()
        {
            Application.Quit();
        }
    }
}