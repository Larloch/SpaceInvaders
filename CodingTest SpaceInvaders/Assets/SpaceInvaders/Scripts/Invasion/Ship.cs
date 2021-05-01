using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Scripts.Invasion
{
    public class Ship : MonoBehaviour
    {
        /// <summary>
        ///     The prefab of the projectile.
        /// </summary>
        [SerializeField] private ShipProjectile shipProjectilePrefab;

        /// <summary>
        ///     The sprite renderer of the Ship.
        /// </summary>
        [SerializeField] public SpriteRenderer ShipSpriteRenderer;

        /// <summary>
        ///     Reduce the horizontal speed of the player ship.
        /// </summary>
        public const float SPEED_REDUCER = 4f;

        /// <summary>
        ///     This value is used to calculate the intervall between a shot and 
        ///     the following one (in seconds).
        /// </summary>
        public const float RECHARGE_DURATION = 0.4f;

        /// <summary>
        ///     Flag enabled when the next shot is available.
        /// </summary>
        private bool shotAvailable = true;

        /// <summary>
        ///     Countdown to the next projectile recharge.
        /// </summary>
        private float rechargingTime = RECHARGE_DURATION;

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
                        Mathf.Min(transform.position.x + (Input.GetAxis("Horizontal") / SPEED_REDUCER), 
                            InvasionManager.Instance.RightBorderPosition),
                        transform.position.y,
                        transform.position.z
                    );
                }
                else if (Input.GetAxis("Horizontal") < -0.01f)
                {
                    transform.position = new Vector3(
                        Mathf.Max(transform.position.x + (Input.GetAxis("Horizontal") / SPEED_REDUCER), 
                            InvasionManager.Instance.LeftBorderPosition),
                        transform.position.y,
                        transform.position.z
                    );
                }
                if (!shotAvailable)
                {
                    rechargingTime -= Time.fixedDeltaTime;
                    if (rechargingTime <= 0f)
                    {
                        rechargingTime = RECHARGE_DURATION;
                        shotAvailable = true;
                    }
                }    
            }
        }

        private void Update()
        {
            if (InvasionManager.Instance.CurrentPhase == InvasionManager.GamePhase.Play && 
                shotAvailable && Input.GetButtonDown("Fire"))
            {                
                Instantiate(shipProjectilePrefab, transform.position, Quaternion.identity);
                shotAvailable = false;   
            }
        }
    }
}