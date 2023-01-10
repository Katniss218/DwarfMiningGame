using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        [field: SerializeField]
        public Transform FollowTarget { get; set; }

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