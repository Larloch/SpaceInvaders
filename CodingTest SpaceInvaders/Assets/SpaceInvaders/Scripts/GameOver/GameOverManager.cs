using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceInvaders.Scripts.GameOver
{
    public class GameOverManager : MonoBehaviour
    {
        /// <summary>
        ///     Called when the player clicks on the restart button
        /// </summary>
        public void OnRestartButtonClick()
        {
            SceneManager.LoadScene("Invasion");
        }

        /// <summary>
        ///     Called when the player clicks on the main menu button
        /// </summary>
        public void OnMainMenuButtonClick()
        {
            Debug.Log("Loading Main Menu");
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