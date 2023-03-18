using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.Player
{
    /// <summary>
    /// Controls the position and orientation of the game camera.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        /// <summary>
        /// The camera will focus on and follow this object.
        /// </summary>
        [field: SerializeField]
        public Transform FollowTarget { get; set; }

        /// <summary>
        /// The offset applied to the following object's position.
        /// </summary>
        [field: SerializeField]
        public Vector3 Offset { get; set; }

        void Update()
        {
            this.transform.position = FollowTarget.position + Offset;
            this.transform.up = Vector3.up;

            this.transform.forward = FollowTarget.transform.position - this.transform.position;
        }
    }
}