using SpaceInvaders.Scripts.Scores;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceInvaders.Scripts.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        /// <summary>
        ///     The warning message visualized if the player
        ///     try to start the game without inserting a name.
        /// </summary>
        [SerializeField] private TMP_Text missingName;

        /// <summary>
        ///     The warning message visualized if the player
        ///     try to start the game without inserting a name.
        /// </summary>
        [SerializeField] private TMP_InputField playerName;

        /// <summary>
        ///     Load the player name from the previous game.
        /// </summary>
        void Start()
        {
            playerName.text = ScoreManager.Instance.PlayerName;
        }

        /// <summary>
        ///     Called when the player clicks on the start button
        /// </summary>
        public void OnStartButtonClick()
        {
            if (playerName.text.Length == 0)
            {
                missingName.gameObject.SetActive(true);
                return;
            }
            if (ScoreManager.Instance.PlayerName != playerName.text)
            {
                ScoreManager.Instance.PlayerName = playerName.text;
                ScoreManager.Instance.HighScore = 0;
            }
            ScoreManager.Instance.CurrentScore = 0;
            ScoreManager.Instance.CurrentLevel = 1;
            SceneManager.LoadScene("Invasion");
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