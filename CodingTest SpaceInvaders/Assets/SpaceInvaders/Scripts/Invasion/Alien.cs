using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Scripts.Invasion
{
    /// <summary>
    ///     Class that represent an alien.
    /// </summary>
    public class Alien : MonoBehaviour
    {
        /// <summary>
        ///     The prefab of the projectile.
        /// </summary>
        [SerializeField] private AlienProjectile alienProjectilePrefab;

        /// <summary>
        ///     The quantity of points gained destroying this alien.
        /// </summary>
        private const int ALIEN_POINTS = 100;

        /// <summary>
        ///     The initial amount of HP of this alien.
        ///     Note: In the current version of the game this quantity is the same 
        ///     removed by a player projectile.
        /// </summary>
        private const int ALIEN_HEALTH_POINTS = 100;

        /// <summary>
        ///     The sprite renderer of the Alien.
        /// </summary>
        public SpriteRenderer AlienSpriteRenderer { get; private set; }

        /// <summary>
        ///     Health of this alien (0 or less and it is dead).
        /// </summary>
        public int HealthPoints { get; private set; }

        /// <summary>
        ///     The closest upper alien in the same column.
        ///     Null if no other aliens are over this one.
        /// </summary>
        public Alien UpperAlien { get; set; }

        /// <summary>
        ///     The closest lower alien in the same column.
        ///     Null if no other aliens are under this one.
        /// </summary>
        public Alien LowerAlien { get; set; }

        /// <summary>
        ///     Initialize the alien health and start the shooting coroutine.
        ///     Note: Since the Aliens move in rows, its movement is managed by the InvasionManager.
        /// </summary>
        void Start()
        {
            AlienSpriteRenderer = GetComponent<SpriteRenderer>();
            HealthPoints = ALIEN_HEALTH_POINTS;
            StartCoroutine(Shoot());
        }

        /// <summary>
        ///     Update the AliensDirection in the InvasionManager if its position is close to
        ///     the borders of the game area.
        /// </summary>
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

        /// <summary>
        ///     Coroutine that manage the shooting frequence of this robot.
        ///     The frequence for the probability check to choose if shoot or not depend on the alien speed.
        /// </summary>
        private IEnumerator Shoot()
        {
            while (HealthPoints > 0)
            {
                if (InvasionManager.Instance.CurrentPhase == InvasionManager.GamePhase.Play && 
                    LowerAlien == null && Random.Range(0, InvasionManager.Instance.AliensShootingRange) == 0)
                {
                    Instantiate(alienProjectilePrefab, transform.position, Quaternion.identity);                    
                }
                yield return new WaitForSeconds(1 / InvasionManager.Instance.AliensSpeed);
            }
        }

        /// <summary>
        ///     Move on the X axis the alien to the given amount of space.
        ///     Called by the InvasionManager.
        /// </summary>
        /// <param name="space">The amount of space (negative to move to the left)</param>
        public void MoveHorizontally(float space)
        {
            transform.position = new Vector3(
                            transform.position.x + space,
                            transform.position.y,
                            0f);
        }

        /// <summary>
        ///     Move on the Y axis the alien to the given amount of space.
        ///     Called by the InvasionManager.
        ///     If the robot reach the level of the blocks is GameOver.
        /// </summary>
        /// <param name="space"></param>
        public void MoveVertically(float space)
        {
            transform.position = new Vector3(
                            transform.position.x,
                            transform.position.y - space,
                            0f);
            if (transform.position.y <= -Camera.main.orthographicSize + InvasionManager.BLOCKS_HEIGHT)
            {
                InvasionManager.Instance.GameOver();
            }
        }

        /// <summary>
        ///     Called when a player projectile hit this alien.
        /// </summary>
        public void OnHit()
        {
            HealthPoints -= 100;
            if (HealthPoints <= 0)
            {
                Die();
            }
        }

        /// <summary>
        ///     Handle the alien destruction.   
        /// </summary>
        private void Die()
        {
            if (UpperAlien != null)
            {
                UpperAlien.LowerAlien = LowerAlien;
            }
            InvasionManager.Instance.RemoveOneAlien();
            UserInterfaceManager.Instance.AddPoints(ALIEN_POINTS);
            gameObject.SetActive(false);
        }


        /// <summary>
        ///     Handle the collision with the blocks or eventually with the player.
        ///     Note: Currently the GameOver is triggered before reaching the player position.
        /// </summary>
        /// <param name="col">The collider of the other object.</param>
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Block"))
            {
                col.GetComponent<Block>().AlienCollision();
                HealthPoints = 0;
                Die();
                return;
            }

            if (col.gameObject.CompareTag("Player"))
            {
                InvasionManager.Instance.GameOver();
            }
        }
    }
}