using SpaceInvaders.Scripts.Scores;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceInvaders.Scripts.Invasion
{
    /// <summary>
    ///     Singleton class that manage the UI of the Invasion scene.
    /// </summary>
    public class UserInterfaceManager : MonoBehaviour, IGameService
    {
        #region SerializedFields

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

        #endregion

        /// <summary>
        ///     The title of the pause panel.
        /// </summary>
        private const string PAUSE_TITLE = "PAUSE";

        /// <summary>
        ///     The message of the pause panel.
        /// </summary>
        private const string PAUSE_MESSAGE = "PRESS: FIRE TO RESUME, ESC TO LEAVE";

        /// <summary>
        ///     Attach the reference of this instance for its access as a singleton.
        /// </summary>
        void Awake()
        {
            ServiceLocator.Register(this);
        }

        /// <summary>
        ///     Request from the ScoreManager the current player name, its previous score,
        ///     and the hi-score and name of the best player. Then add this informations to
        ///     the UI header.
        /// </summary>
        void Start()
        {
            playerName.text = ServiceLocator.Get<ScoreManager>().PlayerName;
            if (ServiceLocator.Get<ScoreManager>().HighScoreOwner != null)
            {
                highScoreOwner.text = ServiceLocator.Get<ScoreManager>().HighScoreOwner;
            }
            currentScore.text = ServiceLocator.Get<ScoreManager>().CurrentScore.ToString();
            highScore.text = ServiceLocator.Get<ScoreManager>().HighScore.ToString();
            SetLevel(ServiceLocator.Get<ScoreManager>().CurrentLevel);
            Cursor.visible = false;
        }

        /// <summary>
        ///     Every time a robot get destroyed this is called to handle the visualization
        ///     of the score (it also update the current score, and eventually the high score
        ///     in the ScoreManager).
        /// </summary>
        /// <param name="points">The quantity of points to add.</param>
        public void AddPoints(int points)
        {
            ServiceLocator.Get<ScoreManager>().CurrentScore = ServiceLocator.Get<ScoreManager>().CurrentScore + points;
            currentScore.text = ServiceLocator.Get<ScoreManager>().CurrentScore.ToString();
            if (ServiceLocator.Get<ScoreManager>().CurrentScore > ServiceLocator.Get<ScoreManager>().HighScore)
            {
                ServiceLocator.Get<ScoreManager>().HighScore = ServiceLocator.Get<ScoreManager>().CurrentScore;
                highScore.text = ServiceLocator.Get<ScoreManager>().HighScore.ToString();
                highScoreOwner.text = ServiceLocator.Get<ScoreManager>().PlayerName;
                ServiceLocator.Get<ScoreManager>().HighScoreOwner = ServiceLocator.Get<ScoreManager>().PlayerName;
            }
        }

        /// <summary>
        ///     Set the UI visualization of the current level.
        /// </summary>
        /// <param name="level">The current level to visualize.</param>
        public void SetLevel(int level)
        {
            centralTitle.text = "LEVEL - " + level;
        }

        /// <summary>
        ///     Close the central panel.
        /// </summary>
        public void CloseCentral()
        {
            centralPanel.SetActive(false);
        }

        /// <summary>
        ///     Open the central panel and set the Pause title and message.
        /// </summary>
        public void OpenPause()
        {
            centralTitle.text = PAUSE_TITLE;
            centralMessage.text = PAUSE_MESSAGE;
            centralPanel.SetActive(true);
        }
    }
}
