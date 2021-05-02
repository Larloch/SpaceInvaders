using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Scripts.Invasion
{
    /// <summary>
    ///     Class that represent the player ship bullet, it handle its movement and its collisions.
    /// </summary>
    public class ShipProjectile : MonoBehaviour
    {
        /// <summary>
        ///     The projectile speed will depend on the AlienSpeed divided by this value.
        /// </summary>
        public const float PROJECTILE_SPEED_REDUCTOR = 20f;

        /// <summary>
        ///     The rigidbody of this projectile.
        /// </summary>
        private Rigidbody2D projectileRigidbody;

        /// <summary>
        ///     Detect and save the reference of this projectile rigidbody.
        /// </summary>
        void Start()
        {
            projectileRigidbody = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        ///     Every fixed time frame (0.02 sec) if the game is started and not in pause will move the projectile.
        /// </summary>
        void FixedUpdate()
        {
            if (InvasionManager.Instance.CurrentPhase == InvasionManager.GamePhase.Play)
            {
                projectileRigidbody.MovePosition(new Vector2(
                    projectileRigidbody.position.x,
                    projectileRigidbody.position.y + (InvasionManager.Instance.AliensSpeed / PROJECTILE_SPEED_REDUCTOR)));
                if (transform.position.y > InvasionManager.Instance.HigherBorderPosition)
                {
                    Destroy(gameObject);
                }
            }
        }

        /// <summary>
        ///     Handle the collision with other objects.
        /// </summary>
        /// <param name="col">The collider of the other object.</param>
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Block"))
            {
                Destroy(gameObject);
                return;
            }

            if (col.gameObject.CompareTag("Alien"))
            {
                col.GetComponent<Alien>().OnHit();
                Destroy(gameObject);
            }
        }
    }
}
