using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Scripts.Invasion
{
    public class Block : MonoBehaviour
    {
        /// <summary>
        ///     The number of hits that a block can absorb.
        /// </summary>
        private const int STRUCTURE_POINTS = 4;

        /// <summary>
        ///     Thickness lost after each hit;
        /// </summary>
        private const float THICKNESS_LOSS = 0.8f;

        /// <summary>
        ///     Position adjustment after hit;
        /// </summary>
        private const float POSITION_ADJUSTMENT = 0.15f;

        /// <summary>
        ///     Current resistance of the block.
        /// </summary>
        private int resistance = STRUCTURE_POINTS;

        /// <summary>
        ///     When an alien projectile hit this block.
        /// </summary>
        public void OnHit()
        {
            resistance--;
            if (resistance > 0)
            {
                transform.localScale = new Vector3(
                    transform.localScale.x,
                    transform.localScale.y - THICKNESS_LOSS,
                    0f);
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y - POSITION_ADJUSTMENT,
                    0f);
                return;
            }
            Destroy(gameObject);
        }
    }
}