using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Items
{
    [CreateAssetMenu( fileName = "item", menuName = "DwarfMiningGame/Item", order = 30 )]
    public class Item : ScriptableObject
    {
        [field: SerializeField]
        public string ID { get; set; }

        [field: SerializeField]
        public float Size { get; set; } = 1.0f;

        [field: SerializeField]
        public Mesh Mesh { get; set; }

        [field: SerializeField]
        public Material Material { get; set; }
    }
}