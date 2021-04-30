using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Scripts.Invasion
{
    public class InvasionManager : MonoBehaviour
    {
        /// <summary>
        ///     The prefab of the aliens.
        /// </summary>
        [SerializeField] private Alien alienPrefab;


        /// <summary>
        ///     The prefab of the aliens.
        /// </summary>
        [SerializeField] private Block blockPrefab;

        /// <summary>
        ///     Height of the top header (it will contains the highscores).
        /// </summary>
        public const float HEADER_HEIGHT = 1f;

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
        private const float ALIENS_HORIZONTAL_OCCUPATION = 0.8f;

        /// <summary>
        ///     The percentage of the vertical space occupied by the aliens at the beginning.
        /// </summary>
        private const float ALIENS_VERTICAL_OCCUPATION = 0.35f;

        /// <summary>
        ///     Vertical distance from the bottom of the Blocks.
        /// </summary>
        private const float BLOCKS_HEIGHT = 1.8f;

        private static float _leftBorderPosition;
        /// <summary>
        ///     Left border position according to the current resolution.
        /// </summary>
        public static float LeftBorderPosition
        {
            get
            {
                if (_leftBorderPosition == 0)
                {
                    _leftBorderPosition = -Camera.main.orthographicSize * Camera.main.aspect;
                }
                return _leftBorderPosition;
            }
        }

        private static float _rightBorderPosition;
        /// <summary>
        ///     Right border position according to the current resolution.
        /// </summary>
        public static float RightBorderPosition
        {
            get
            {
                if (_rightBorderPosition == 0)
                {
                    _rightBorderPosition = Camera.main.orthographicSize * Camera.main.aspect;
                }
                return _rightBorderPosition;
            }
        }

        private static float _higherBorderPosition;
        /// <summary>
        ///     Higher border position of the gameplay area, according to the current resolution.
        /// </summary>
        public static float HigherBorderPosition
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

        private static float _lowerBorderPosition;
        /// <summary>
        ///     Lower border position of the gameplay area, according to the current resolution.
        /// </summary>
        public static float LowerBorderPosition
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
        ///     All the spawned aliens [row][column].
        ///     The rows are from the higher to the lower.
        ///     The columns, from left to right.
        /// </summary>
        private List<List<Alien>> aliensGroup;

        /// <summary>
        ///     List of the blocks.
        /// </summary>
        private List<Block> blocksGroup;

        void Awake()
        {
            SpawnAliens();
            SpawnBlocks();
        }

        private void SpawnAliens()
        {
            aliensGroup = new List<List<Alien>>();
            float horizontalSpace = (Camera.main.orthographicSize * Camera.main.aspect * 2f) * ALIENS_HORIZONTAL_OCCUPATION;
            float horizontalDistance = horizontalSpace / (ALIENS_COLUMNS - 1);
            float verticalSpace = (HigherBorderPosition - LowerBorderPosition) * ALIENS_VERTICAL_OCCUPATION;
            float verticalDistance = verticalSpace / (ALIENS_ROWS - 1);
            float initialHorizontalPosition = -horizontalSpace / 2f;
            float initialVerticalPosition = HigherBorderPosition - 1f; // TODO: Set the height of the Alien sprite.
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
                        Quaternion.identity).GetComponent<Alien>());
                }
            }
        }

        private void SpawnBlocks()
        {
            blocksGroup = new List<Block>();
            float horizontalSpace = Camera.main.orthographicSize * Camera.main.aspect;
            float horizontalDistance = horizontalSpace / (BLOCKS_NUMBER - 1);
            float initialHorizontalPosition = -horizontalSpace / 2f;
            for (int block = 0; block < BLOCKS_NUMBER; ++block)
            {
                blocksGroup.Add(Instantiate(blockPrefab,
                    new Vector3(
                        initialHorizontalPosition + (block * horizontalDistance),
                        LowerBorderPosition + BLOCKS_HEIGHT,
                        0f),
                    Quaternion.identity).GetComponent<Block>());
            }
        }

        private void FixedUpdate()
        {
            
        }

    }
}