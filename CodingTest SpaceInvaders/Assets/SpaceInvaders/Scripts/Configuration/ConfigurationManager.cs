using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Scripts.Configuration
{
    public class ConfigurationManager : MonoBehaviour
    {
        /// <summary>
        ///     ConfigurationManager is a singleton.
        /// </summary>
        public static ConfigurationManager Instance { get; private set; }

        /// <summary>
        ///     Filename (without extension) of the json configuration file.
        /// </summary>
        private const string CONFIGURATION_FILE_NAME = "Configuration";

        /// <summary>
        ///     Current player.
        /// </summary>
        private InvasionConfiguration invasionConfig;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            invasionConfig = JsonUtility.FromJson<InvasionConfiguration>(Resources.Load<TextAsset>(CONFIGURATION_FILE_NAME).ToString());
        }

        public float GetCurrentSpeed(int level)
        {
            return Mathf.Min(
                invasionConfig.StartingSpeed + ((level-1) * invasionConfig.SpeedIncrease), 
                invasionConfig.SpeedLimit);
        }

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
            // TODO: Comment
            public float StartingSpeed;
            public float SpeedIncrease;
            public float SpeedLimit;
            public int StartingShooting;
            public int ShootingIncrease;
            public int ShootingLimit;
        }
    }
}