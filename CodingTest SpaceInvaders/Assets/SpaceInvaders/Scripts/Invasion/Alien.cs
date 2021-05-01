using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Scripts.Invasion
{
    public class Alien : MonoBehaviour
    {
        /// <summary>
        ///     The prefab of the projectile.
        /// </summary>
        [SerializeField] private AlienProjectile alienProjectilePrefab;

        public const int BASE_SHOOTING_CHANCE = 100;

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
        public Alien UpperAlien;

        /// <summary>
        ///     The closest lower alien in the same column.
        ///     Null if no other aliens are under this one.
        /// </summary>
        public Alien LowerAlien;

        void Start()
        {
            AlienSpriteRenderer = GetComponent<SpriteRenderer>();
            HealthPoints = 100;
            StartCoroutine(Shoot());
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

        private IEnumerator Shoot()
        {
            while (HealthPoints > 0)
            {
                if (InvasionManager.Instance.CurrentPhase == InvasionManager.GamePhase.Play && LowerAlien == null)
                {
                    if (Random.Range(0, BASE_SHOOTING_CHANCE) == 0)
                    {
                        Instantiate(alienProjectilePrefab, transform.position, Quaternion.identity);
                    }
                }
                yield return new WaitForSeconds(1 / InvasionManager.Instance.AliensSpeed);
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