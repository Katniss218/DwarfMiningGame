using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Inventories
{
    [CreateAssetMenu( fileName = "item", menuName = "DwarfMiningGame/Item", order = 300 )]
    public class Item : ScriptableObject, IIdentifiable
    {
        public static float SellValueMultiplier = 0.25f;

        [field: SerializeField]
        public string ID { get; set; }

        /// <summary>
        /// The amount of inventory volume/mass taken up by this item.
        /// </summary>
        public float Size = 1.0f;

        /// <summary>
        /// The monetary value of this item when buying from a shop.
        /// </summary>
        [field: SerializeField]
        public float Value { get; set; } = 0.0f;

        /// <summary>
        /// The monetary value of this item when selling to a shop.
        /// </summary>
        public float SellValue { get => this.Value * SellValueMultiplier; }

        public Mesh Mesh;

        public Material Material;

        public Sprite Icon;
    }
}