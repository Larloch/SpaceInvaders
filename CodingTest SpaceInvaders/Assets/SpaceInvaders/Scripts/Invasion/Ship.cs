using UnityEngine;

namespace SpaceInvaders.Scripts.Invasion
{
    /// <summary>
    ///     Class that represent the player ship.
    /// </summary>
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
        public const float RELOAD_DURATION = 0.4f;

        /// <summary>
        ///     Flag enabled when the next shot is available.
        /// </summary>
        private bool shotAvailable = true;

        /// <summary>
        ///     Countdown to the next projectile recharge.
        /// </summary>
        private float reloadingTime = RELOAD_DURATION;

        /// <summary>
        ///     Handle the player movement inputs.
        /// </summary>
        void FixedUpdate()
        {
            if (InvasionManager.Instance.CurrentPhase == InvasionManager.GameStates.Play)
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
            }
        }

        /// <summary>
        ///     Handle the shooting input.
        ///     This can stay in the regular Update because the shots have a reload time.
        /// </summary>
        void Update()
        {
            if (InvasionManager.Instance.CurrentPhase == InvasionManager.GameStates.Play)
            {
                if (!shotAvailable)
                {
                    reloadingTime -= Time.deltaTime;
                    if (reloadingTime <= 0f)
                    {
                        reloadingTime = RELOAD_DURATION;
                        shotAvailable = true;
                    }
                }
                if (shotAvailable && Input.GetButtonDown("Fire"))
                {
                    Instantiate(shipProjectilePrefab, transform.position, Quaternion.identity);
                    shotAvailable = false;
                }
            }
        }
    }
}
