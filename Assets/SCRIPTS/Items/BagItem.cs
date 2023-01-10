using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Items
{
    /// A bag is an item that can increase your inventory size.
    [CreateAssetMenu( fileName = "bag item", menuName = "DwarfMiningGame/Item-Bag", order = 32 )]
    public class BagItem : Item
    {
        [field: SerializeField]
        public float MaxCapacity { get; set; }
    }
}