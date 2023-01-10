using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Tiles
{
    public class Mineral : MonoBehaviour, IHardnessModifier
    {
        // resources contain type and amount
        // resources are embedded in a tile
        // resources might affect tile's time to break

        [field: SerializeField]
        public float Hardness { get; set; }
    }
}