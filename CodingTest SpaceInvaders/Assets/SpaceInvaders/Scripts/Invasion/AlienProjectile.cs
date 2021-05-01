using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Scripts.Invasion
{
    public class AlienProjectile : MonoBehaviour
    {
        /// <summary>
        ///     The projectile speed will depend on the AlienSpeed divided by this value.
        /// </summary>
        public const float PROJECTILE_SPEED_REDUCTOR = 100f;

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
                    projectileRigidbody.position.y - (InvasionManager.Instance.AliensSpeed / PROJECTILE_SPEED_REDUCTOR)));
                if (transform.position.y < InvasionManager.Instance.LowerBorderPosition)
                {
                    Destroy(gameObject);
                }
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag == "Block")
            {
                col.GetComponent<Block>().OnHit();
                Destroy(gameObject);
                return;
            }

            if (col.gameObject.tag == "Player")
            {
                InvasionManager.Instance.GameOver();
                Destroy(gameObject);
            }
        }
    }
}
