using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Items
{
    [CreateAssetMenu( fileName = "item", menuName = "DwarfMiningGame/Item", order = 300 )]
    public class Item : ScriptableObject, IIdentifiable
    {
        public static float SellValueMultiplier = 0.25f;

        [field: SerializeField]
        public string ID { get; set; }

        public float Size = 1.0f;

        [field: SerializeField]
        public float Value { get; set; } = 0.0f;

        public Mesh Mesh;

        public Material Material;

        public Sprite Icon;
    }
}