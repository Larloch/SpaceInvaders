using System;
using UnityEngine;

namespace SpaceInvaders.Scripts.Configuration
{
    /// <summary>
    ///     Singleton class that is used to load the configuration from the json file (saved in the resources folder).
    ///     This is attached to its gameobject in the MainMenu scene, but it will be not destroyed on load.
    /// </summary>
    public class ConfigurationManager : MonoBehaviour, IGameService
    {
        /// <summary>
        ///     Filename (without extension) of the json configuration file.
        /// </summary>
        private const string CONFIGURATION_FILE_NAME = "Configuration";

        /// <summary>
        ///     Current player.
        /// </summary>
        private InvasionConfiguration invasionConfig;

        /// <summary>
        ///     Attach the reference of this instance for its access as a singleton and load
        ///     the configuration from the json file in the resources folder.
        /// </summary>
        void Awake()
        {
            ServiceLocator.Register(this);
            DontDestroyOnLoad(gameObject);
            invasionConfig = JsonUtility.FromJson<InvasionConfiguration>(Resources.Load<TextAsset>(CONFIGURATION_FILE_NAME).ToString());
        }

        /// <summary>
        ///     Expose the aliens speed parameter of the configuration.
        ///     The speed is calculated considering the current level, the initial speed and the maximum speed.
        /// </summary>
        /// <param name="level">The current level</param>
        /// <returns>The aliens speed relative to the current level.</returns>
        public float GetCurrentSpeed(int level)
        {
            return Mathf.Min(
                invasionConfig.StartingSpeed + ((level-1) * invasionConfig.SpeedIncrease), 
                invasionConfig.SpeedLimit);
        }

        /// <summary>
        ///     Expose the aliens shooting frequence range parameter of the configuration.
        ///     The range is calculated considering the current level, the initial range, and the range limit.
        /// </summary>
        /// <param name="level">The current level</param>
        /// <returns>The aliens speed relative to the current level.</returns>
        public int GetCurrentShootingRange(int level)
        {
            return Mathf.Max(
                invasionConfig.StartingShooting - ((level - 1) * invasionConfig.ShootingIncrease), 
                invasionConfig.ShootingLimit);
        }

        /// <summary>
        ///     This serialization of the configuration json used to setup the aliens speed and attacks.
        /// </summary>
        [Serializable]
        public class InvasionConfiguration
        {
            /// <summary>
            ///     The initial aliens speed selected in the firs level.
            /// </summary>
            public float StartingSpeed;
            
            /// <summary>
            ///     The value of the alien movement speed increased each level.
            /// </summary>
            public float SpeedIncrease;
            
            /// <summary>
            ///     The maximum speed reachable.
            /// </summary>
            public float SpeedLimit;
            
            /// <summary>
            ///     The initial upper bound of the range used to choose the probability
            ///     that an alien has to shoot. This probability is calculated in fixed timeframes depending
            ///     on the speed of the alien (higher speed means more frequent probability checks).
            ///     E.g.: With the StartingShooting value set to 50, during the first level each available 
            ///     alien will shoot with a probability of 1 / 50.
            /// </summary>
            public int StartingShooting;
            
            /// <summary>
            ///     This is the quantity of range that is removed by the current range to increase
            ///     the probability of the aliens to shoot.
            ///     E.g.: If in the previous level the range was 50, and the value of ShootingIncrease
            ///     is 5, the next level the probability to shoot is 1 / 45.
            /// </summary>
            public int ShootingIncrease;
            
            /// <summary>
            ///     The minimum value of the range used to choose if the alien can shoot every fixed time frame.
            /// </summary>
            public int ShootingLimit;
        }
    }
}
