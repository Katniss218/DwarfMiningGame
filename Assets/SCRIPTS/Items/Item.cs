using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Items
{
    [CreateAssetMenu( fileName = "item", menuName = "DwarfMiningGame/Item", order = 30 )]
    public class Item : ScriptableObject, IIdentifiable
    {
        [field: SerializeField]
        public string ID { get; set; }

        public float Size = 1.0f;

        public Mesh Mesh;

        public Material Material;

        public Sprite Icon;
    }
}