using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Scripts.Invasion
{
    public class Alien : MonoBehaviour
    {
        /// <summary>
        ///     The sprite renderer of the Alien.
        /// </summary>
        public SpriteRenderer AlienSpriteRenderer { get; private set; }

        void Start()
        {
            AlienSpriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}