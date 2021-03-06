using UnityEngine;

namespace SpaceInvaders.Scripts.Invasion
{
    /// <summary>
    ///     Class that represent the alien bullet, it handle its movement and its collisions.
    /// </summary>
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
            if (InvasionManager.Instance.CurrentState == InvasionManager.GameStates.Play)
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

        /// <summary>
        ///     Handle the collision with other objects.
        /// </summary>
        /// <param name="col">The collider of the other object.</param>
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Block"))
            {
                col.GetComponent<Block>().OnAlienHit();
                Destroy(gameObject);
                return;
            }

            if (col.gameObject.CompareTag("Projectile"))
            {
                Destroy(col.gameObject);
                Destroy(gameObject);
            }

            if (col.gameObject.CompareTag("Player"))
            {
                InvasionManager.Instance.GameOver();
                Destroy(gameObject);
            }
        }
    }
}
