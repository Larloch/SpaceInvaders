using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Scripts.Invasion
{
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

        void Start()
        {
            projectileRigidbody = GetComponent<Rigidbody2D>();
        }

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

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag == "Block")
            {
                Destroy(gameObject);
                return;
            }

            if (col.gameObject.tag == "Alien")
            {
                col.GetComponent<Alien>().OnHit();
                Destroy(gameObject);
            }
        }
    }
}
