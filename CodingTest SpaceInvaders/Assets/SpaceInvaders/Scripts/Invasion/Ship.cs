using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Scripts.Invasion
{
    public class Ship : MonoBehaviour
    {
        void Start()
        {

        }

        /// <summary>
        ///     Handle the player inputs
        /// </summary>
        void FixedUpdate()
        {
            if (Input.GetAxis("Horizontal") > 0.01f)
            {
                transform.position = new Vector3(
                    Mathf.Min(transform.position.x + (Input.GetAxis("Horizontal") / 2f), 8f),
                    transform.position.y,
                    transform.position.z
                );
            }
            else if (Input.GetAxis("Horizontal") < -0.01f)
            {
                transform.position = new Vector3(
                    Mathf.Max(transform.position.x + (Input.GetAxis("Horizontal") / 2f), -8f),
                    transform.position.y,
                    transform.position.z
                );
            }
        }
    }
}