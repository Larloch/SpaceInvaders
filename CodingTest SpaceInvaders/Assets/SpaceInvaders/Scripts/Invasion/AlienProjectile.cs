using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Scripts.Invasion
{
    public class AlienProjectile : MonoBehaviour
    {
        void FixedUpdate()
        {
            if (InvasionManager.Instance.CurrentPhase == InvasionManager.GamePhase.Play)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y - (InvasionManager.Instance.AliensSpeed / 100f),
                    0f);
                if (transform.position.y < InvasionManager.Instance.LowerBorderPosition)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
