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

        void Update()
        {
            if (InvasionManager.Instance.CurrentPhase == InvasionManager.GamePhase.Play)
            {
                if (transform.position.x <= InvasionManager.Instance.LeftBorderPosition)
                {
                    InvasionManager.Instance.AliensDirection = InvasionManager.Direction.Right;
                }
                else if (transform.position.x >= InvasionManager.Instance.RightBorderPosition)
                {
                    InvasionManager.Instance.AliensDirection = InvasionManager.Direction.Left;
                }
            }
        }

        public void MoveHorizontally(float space)
        {
            transform.position = new Vector3(
                            transform.position.x + space,
                            transform.position.y,
                            0f);
        }

        public void MoveVertically(float space)
        {
            transform.position = new Vector3(
                            transform.position.x,
                            transform.position.y - space,
                            0f);
        }
    }
}