using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Inventories
{ 
    [CreateAssetMenu( fileName = "pickaxe item", menuName = "DwarfMiningGame/Item (Pickaxe)", order = 310 )]
    public class ItemPickaxe : ItemEquippable
    {
        [field: SerializeField]
        public float MiningSpeed { get; set; }

        [field: SerializeField]
        public float MaxHardness { get; set; }

        public override void OnEquip( Inventory i )
        {
            throw new System.NotImplementedException();
        }
    }
}