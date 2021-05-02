using SpaceInvaders.Scripts.Scores;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceInvaders.Scripts.GameOver
{
    /// <summary>
    ///     Class used in the GameOver scene to manage
    ///     the button clicks and to display the score in the last game.
    /// </summary>
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

        /// <summary>
        ///     Display the score of the player in the last game.
        ///     Request also to save it in the leaderbord 
        ///     (if the score is too low it will not be saved).
        /// </summary>
        void Start()
        {
            playerName.text = ScoreManager.Instance.PlayerName;
            currentScore.text = ScoreManager.Instance.CurrentScore.ToString();
            ScoreManager.Instance.SaveFinalScore();
            Cursor.visible = true;
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