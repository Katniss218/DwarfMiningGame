using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Inventories
{ 
    [CreateAssetMenu( fileName = "pickaxe item", menuName = "DwarfMiningGame/Item (Pickaxe)", order = 310 )]
    public class ItemPickaxe : Item, IItemEquippable
    {
        [field: SerializeField]
        public float MiningSpeed { get; set; }

        [field: SerializeField]
        public float MaxHardness { get; set; }

        public void OnEquip( Equipment eq )
        {
            // player should swing whatever is in here.
        }

        public void OnUnequip( Equipment eq )
        {
            
        }
    }
}