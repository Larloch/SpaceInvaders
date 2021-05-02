using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceInvaders.Scripts.Scores
{
    public class LeaderboardsManager : MonoBehaviour
    {
        /// <summary>
        ///     The size of the leaderboard.
        /// </summary>
        private const int LEADERBOARD_SIZE = 3;

        /// <summary>
        ///     List of TMP_Text of the score owners, ordered from
        ///     the top score player to the last.
        /// </summary>
        [SerializeField] private List<TMP_Text> ScoreOwners;

        /// <summary>
        ///     List of TMP_Text of the score owners, ordered from
        ///     the top score to the last.
        /// </summary>
        [SerializeField] private List<TMP_Text> Scores;

        void Start()
        {
            List<(string, int)> topPlayers = ScoreManager.Instance.GetLeaderboardTop(LEADERBOARD_SIZE);
            for (int position = 0; position < topPlayers.Count; ++position)
            {
                ScoreOwners[position].text = topPlayers[position].Item1;
                Scores[position].text = topPlayers[position].Item2.ToString();
            }
        }

        public void OnMainMenuButtonClick()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}