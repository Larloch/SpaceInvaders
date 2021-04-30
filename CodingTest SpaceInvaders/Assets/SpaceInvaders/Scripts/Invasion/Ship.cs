using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Scripts.Invasion
{
    public class Ship : MonoBehaviour
    {
        /// <summary>
        ///     The sprite renderer of the Alien.
        /// </summary>
        [SerializeField] public SpriteRenderer ShipSpriteRenderer;

        /// <summary>
        ///     Handle the player inputs
        /// </summary>
        void FixedUpdate()
        {
            if (InvasionManager.Instance.CurrentPhase == InvasionManager.GamePhase.Play)
            {
                if (Input.GetAxis("Horizontal") > 0.01f)
                {
                    transform.position = new Vector3(
                        Mathf.Min(transform.position.x + (Input.GetAxis("Horizontal") / 2f), InvasionManager.Instance.RightBorderPosition),
                        transform.position.y,
                        transform.position.z
                    );
                }
                else if (Input.GetAxis("Horizontal") < -0.01f)
                {
                    transform.position = new Vector3(
                        Mathf.Max(transform.position.x + (Input.GetAxis("Horizontal") / 2f), InvasionManager.Instance.LeftBorderPosition),
                        transform.position.y,
                        transform.position.z
                    );
                }
            }
        }
    }
}