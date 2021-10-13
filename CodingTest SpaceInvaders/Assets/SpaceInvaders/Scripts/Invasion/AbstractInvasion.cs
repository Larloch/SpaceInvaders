using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Scripts.Invasion
{
    public abstract class AbstractInvasion : MonoBehaviour, IGameService
    {
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
        ///     The aliens movement direction.
        /// </summary>
        public Direction AliensDirection { get; set; }

        /// <summary>
        ///     Current speed of the aliens.
        /// </summary>
        public float AliensSpeed { get; protected set; }

        /// <summary>
        ///     The upper bound of the random range used to decide if
        ///     an alien will shoot or not.
        /// </summary>
        public int AliensShootingRange { get; protected set; }

        public abstract float LeftBorderPosition { get; }

        public abstract float RightBorderPosition { get; }

        public abstract float HigherBorderPosition { get; }

        public abstract float LowerBorderPosition { get; }

        public abstract void GameOver();

        public abstract void RemoveOneAlien();

        public abstract bool IsInPlayState();

    }
}