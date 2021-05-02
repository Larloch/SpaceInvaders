using SpaceInvaders.Scripts.Configuration;
using SpaceInvaders.Scripts.Scores;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace SpaceInvaders.Scripts.Invasion
{
    /// <summary>
    ///     Singleton class that manage the game progression.
    /// </summary>
    public class InvasionManager : MonoBehaviour
    {
        /// <summary>
        ///     InvasionManager is a singleton.
        /// </summary>
        public static InvasionManager Instance { get; private set; }

        /// <summary>
        ///     The prefab of the aliens.
        /// </summary>
        [SerializeField] private Ship playerShip;

        /// <summary>
        ///     The prefab of the aliens.
        /// </summary>
        [SerializeField] private Alien alienPrefab;

        /// <summary>
        ///     The prefab of the blocks.
        /// </summary>
        [SerializeField] private Block blockPrefab;
        
        /// <summary>
        ///     Enumerator to describe the states of the game.
        /// </summary>
        public enum GameStates
        {
            Start,
            Play,
            Pause
        }

        /// <summary>
        ///     Enumerator for the aliens movement directions.
        /// </summary>
        public enum Direction
        {
            Left,
            Right
        }

        /// <summary>
        ///     Height of the top header (it will contains the highscores).
        /// </summary>
        public const float HEADER_HEIGHT = 1.4f;

        /// <summary>
        ///     Vertical distance from the bottom of the Blocks.
        /// </summary>
        public const float BLOCKS_HEIGHT = 1.8f;

        /// <summary>
        ///     The number of columns of the invasion.
        /// </summary>
        private const int ALIENS_COLUMNS = 11;

        /// <summary>
        ///     The number of rows of the invasion.
        /// </summary>
        private const int ALIENS_ROWS = 5;

        /// <summary>
        ///     Number of the Blocks.
        /// </summary>
        private const int BLOCKS_NUMBER = 4;

        /// <summary>
        ///     The percentage of the horizontal space occupied by the aliens at the beginning.
        /// </summary>
        private const float ALIENS_HORIZONTAL_OCCUPATION = 0.6f;

        /// <summary>
        ///     The percentage of the vertical space occupied by the aliens at the beginning.
        /// </summary>
        private const float ALIENS_VERTICAL_OCCUPATION = 0.35f;

        /// <summary>
        ///     The number of movements that an alien perform each row 
        ///     (when the alien at the border are still alive).
        /// </summary>
        private const int ALIENS_HORIZONTAL_MOVEMENTS = 15;

        /// <summary>
        ///     The number of movements that an alien perform each row.
        /// </summary>
        private const int ALIENS_VERTICAL_MOVEMENTS = 10;

        /// <summary>
        ///     The initial position of the aliens on the y axis.
        /// </summary>
        private const int ALIENS_INITIAL_VERTICAL_POSITION = 3;

        /// <summary>
        ///     Current Game Phase
        /// </summary>
        public GameStates CurrentPhase { get; private set; }

        private float _leftBorderPosition;
        /// <summary>
        ///     Left border position according to the current resolution.
        /// </summary>
        public float LeftBorderPosition
        {
            get
            {
                if (_leftBorderPosition == 0)
                {
                    _leftBorderPosition = (-Camera.main.orthographicSize * Camera.main.aspect) + playerShip.ShipSpriteRenderer.bounds.size.x;
                }
                return _leftBorderPosition;
            }
        }

        private float _rightBorderPosition;
        /// <summary>
        ///     Right border position according to the current resolution.
        /// </summary>
        public float RightBorderPosition
        {
            get
            {
                if (_rightBorderPosition == 0)
                {
                    _rightBorderPosition = (Camera.main.orthographicSize * Camera.main.aspect) - playerShip.ShipSpriteRenderer.bounds.size.x;
                }
                return _rightBorderPosition;
            }
        }

        private float _higherBorderPosition;
        /// <summary>
        ///     Higher border position of the gameplay area, according to the current resolution.
        /// </summary>
        public float HigherBorderPosition
        {
            get
            {
                if (_higherBorderPosition == 0)
                {
                    _higherBorderPosition = Camera.main.orthographicSize - HEADER_HEIGHT;
                }
                return _higherBorderPosition;
            }
        }

        private float _lowerBorderPosition;
        /// <summary>
        ///     Lower border position of the gameplay area, according to the current resolution.
        /// </summary>
        public float LowerBorderPosition
        {
            get
            {
                if (_lowerBorderPosition == 0)
                {
                    _lowerBorderPosition = -Camera.main.orthographicSize;
                }
                return _lowerBorderPosition;
            }
        }

        /// <summary>
        ///     Current speed of the aliens.
        /// </summary>
        public float AliensSpeed { get; private set; }

        /// <summary>
        ///     The upper bound of the random range used to decide if
        ///     an alien will shoot or not.
        /// </summary>
        public int AliensShootingRange { get; private set; }

        /// <summary>
        ///     The aliens movement direction.
        /// </summary>
        public Direction AliensDirection { get; set; }

        /// <summary>
        ///     All the spawned aliens [row][column].
        ///     The rows are from the higher to the lower.
        ///     The columns, from left to right.
        /// </summary>
        private List<List<Alien>> aliensGroup;

        /// <summary>
        ///     GameObject parent of all the Aliens.
        /// </summary>
        private GameObject aliensContainer;

        /// <summary>
        ///     The minimum movement performed by an alien.
        /// </summary>
        private float alienMinimumMovement;

        /// <summary>
        ///     The number of remaining aliens in the game.
        /// </summary>
        private int aliensLeft;

        /// <summary>
        ///     Attach the reference of this instance for its access as a singleton and initialize the game phase.
        /// </summary>
        void Awake()
        {
            Assert.IsNull(Instance, "Only one instance of InvasionManager is allowed");
            Instance = this;
            aliensLeft = ALIENS_COLUMNS * ALIENS_ROWS;
            CurrentPhase = GameStates.Start;
            AliensDirection = Direction.Right;
            SpawnAliens();
            SpawnBlocks();
        }

        /// <summary>
        ///     Open the GameOver panel.
        /// </summary>
        public void GameOver()
        {
            SceneManager.LoadScene("GameOver");
        }

        /// <summary>
        ///     When an alien die decrease the counter to
        ///     determine when the level is won.
        /// </summary>
        public void RemoveOneAlien()
        {
            aliensLeft--;
            if (aliensLeft == 0)
            {
                LevelWon();
            }
        }

        /// <summary>
        ///     Open the GameOver panel.
        /// </summary>
        private void LevelWon()
        {
            ScoreManager.Instance.CurrentLevel++;
            SceneManager.LoadScene("Invasion");
        }

        /// <summary>
        ///     Instantiate all the aliens. The position is calculated considering the screen size, in a way that it should adapt well to
        ///     various resolutions (better if in a landscape format).
        /// </summary>
        private void SpawnAliens()
        {
            aliensGroup = new List<List<Alien>>();
            aliensContainer = Instantiate(new GameObject());
            aliensContainer.name = "AliensContainer";
            float horizontalSpace = (RightBorderPosition - LeftBorderPosition) * ALIENS_HORIZONTAL_OCCUPATION;
            alienMinimumMovement = (RightBorderPosition - LeftBorderPosition) * (1f - ALIENS_HORIZONTAL_OCCUPATION) / ALIENS_HORIZONTAL_MOVEMENTS;
            float horizontalDistance = horizontalSpace / (ALIENS_COLUMNS - 1);
            float verticalSpace = (HigherBorderPosition - LowerBorderPosition) * ALIENS_VERTICAL_OCCUPATION;
            float verticalDistance = verticalSpace / (ALIENS_ROWS - 1);
            float initialHorizontalPosition = -horizontalSpace / 2f;
            float initialVerticalPosition = ALIENS_INITIAL_VERTICAL_POSITION;
            for (int row = 0; row < ALIENS_ROWS; ++row)
            {
                aliensGroup.Add(new List<Alien>());
                for (int column = 0; column < ALIENS_COLUMNS; ++column)
                {
                    aliensGroup[row].Add(Instantiate(alienPrefab,
                        new Vector3(
                            initialHorizontalPosition + (column * horizontalDistance),
                            initialVerticalPosition - (row * verticalDistance),
                            0f),
                        Quaternion.identity, aliensContainer.transform).GetComponent<Alien>());
                    if (row > 0)
                    {
                        aliensGroup[row][column].UpperAlien = aliensGroup[row - 1][column];
                        aliensGroup[row - 1][column].LowerAlien = aliensGroup[row][column];
                    }    
                }
            }
            aliensContainer.SetActive(false);
        }

        /// <summary>
        ///     Instantiate the four protection blocks. 
        /// </summary>
        private void SpawnBlocks()
        {
            float horizontalSpace = Camera.main.orthographicSize * Camera.main.aspect;
            float horizontalDistance = horizontalSpace / (BLOCKS_NUMBER - 1);
            float initialHorizontalPosition = -horizontalSpace / 2f;
            for (int block = 0; block < BLOCKS_NUMBER; ++block)
            {
                Instantiate(blockPrefab,
                    new Vector3(
                        initialHorizontalPosition + (block * horizontalDistance),
                        LowerBorderPosition + BLOCKS_HEIGHT,
                        0f),
                    Quaternion.identity);
            }
        }

        /// <summary>
        ///     Handle the keys to move from a game state to another:
        ///     to start, pause or resume the game.
        /// </summary>
        private void Update()
        {
            switch (CurrentPhase)
            {
                case GameStates.Start:
                    if (Input.GetButtonUp("Fire") || Input.GetButtonUp("Quit"))
                    {
                        StartGame();
                    }
                    break;
                case GameStates.Play:
                    if (Input.GetButtonUp("Quit"))
                    {
                        PauseGame();
                    }
                    break;
                case GameStates.Pause:
                    if (Input.GetButtonUp("Fire"))
                    {
                        ResumeGame();
                    }
                    if (Input.GetButtonUp("Quit"))
                    {
                        GameOver();
                    }
                    break;
            }
        }

        /// <summary>
        ///     Pause the game.
        /// </summary>
        private void PauseGame()
        {
            CurrentPhase = GameStates.Pause;
            aliensContainer.SetActive(false);
            UserInterfaceManager.Instance.OpenPause();
        }

        /// <summary>
        ///     Resume the game from the pause state.
        /// </summary>
        private void ResumeGame()
        {
            UserInterfaceManager.Instance.CloseCentral();
            aliensContainer.SetActive(true);
            CurrentPhase = GameStates.Play;
        }

        /// <summary>
        ///     Start the game and start the coroutine to move the aliens.
        /// </summary>
        private void StartGame()
        {
            ResumeGame();
            AliensSpeed = ConfigurationManager.Instance.GetCurrentSpeed(ScoreManager.Instance.CurrentLevel);
            AliensShootingRange = ConfigurationManager.Instance.GetCurrentShootingRange(ScoreManager.Instance.CurrentLevel);
            StartCoroutine(MoveAliens());
        }

        /// <summary>
        ///     Move the aliens in the typical SpaceInvaders manner (row by row, starting from the lowest one).
        ///     The movement is willingly not smooth to replicate better the SpaceInvaders feeling.
        /// </summary>
        private IEnumerator MoveAliens()
        {
            int currentRow = ALIENS_ROWS - 1;
            int aliensDirection = 1;
            float verticalSpaceLeft = (HigherBorderPosition - LowerBorderPosition) * (1f - ALIENS_VERTICAL_OCCUPATION);
            float alienVerticalMovement = verticalSpaceLeft / ALIENS_VERTICAL_MOVEMENTS;
            bool moveVertical = false;
            while (CurrentPhase != GameStates.Start)
            {
                if (CurrentPhase == GameStates.Play)
                {
                    foreach (Alien alien in aliensGroup[currentRow])
                    {
                        alien.MoveHorizontally(alienMinimumMovement * aliensDirection);
                        if (moveVertical)
                        {
                            alien.MoveVertically(alienVerticalMovement);
                        }
                    }
                    if (--currentRow < 0)
                    {
                        currentRow = ALIENS_ROWS - 1;
                        int newDirection = AliensDirection == Direction.Right ? 1 : -1;
                        if (newDirection != aliensDirection)
                        {
                            aliensDirection = newDirection;
                            moveVertical = true;
                        }
                        else
                        {
                            moveVertical = false;
                        }
                    }
                }
                yield return new WaitForSeconds(1 / AliensSpeed);
            }
        }
    }
}
