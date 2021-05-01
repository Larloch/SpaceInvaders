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
        [HideInInspector] public string PlayerName;

        /// <summary>
        ///     Name of the player that own the Highscore of this session.
        /// </summary>
        [HideInInspector] public string HighScoreOwner;

        /// <summary>
        ///     Highscore of this session.
        /// </summary>
        [HideInInspector] public int HighScore;

        /// <summary>
        ///     Current score of this session.
        /// </summary>
        [HideInInspector] public int CurrentScore;

        /// <summary>
        ///     Current level of this game.
        /// </summary>
        [HideInInspector] public int CurrentLevel;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}