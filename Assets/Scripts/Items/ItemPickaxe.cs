using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Items
{ 
    [CreateAssetMenu( fileName = "pickaxe item", menuName = "DwarfMiningGame/Item (Pickaxe)", order = 310 )]
    public class ItemPickaxe : Item
    {
        [field: SerializeField]
        public float MiningSpeed { get; set; }

        [field: SerializeField]
        public float MaxHardness { get; set; }
    }
}