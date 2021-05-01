using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceInvaders.Scripts.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        /// <summary>
        ///     Called when the player clicks on the start button
        /// </summary>
        public void OnStartButtonClick()
        {
            // TODO: Check that the name of the player is not empty.
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