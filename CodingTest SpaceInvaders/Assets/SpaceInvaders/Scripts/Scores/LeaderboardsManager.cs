using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceInvaders.Scripts.Scores
{
    public class LeaderboardsManager : MonoBehaviour
    {
        void Start()
        {

        }

        public void OnMainMenuButtonClick()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}