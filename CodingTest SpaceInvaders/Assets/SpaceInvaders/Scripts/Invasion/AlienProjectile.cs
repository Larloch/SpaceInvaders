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

        void FixedUpdate()
        {
            if (InvasionManager.Instance.CurrentPhase == InvasionManager.GamePhase.Play)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y - (InvasionManager.Instance.AliensSpeed / PROJECTILE_SPEED_REDUCTOR),
                    0f);
                if (transform.position.y < InvasionManager.Instance.LowerBorderPosition)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
