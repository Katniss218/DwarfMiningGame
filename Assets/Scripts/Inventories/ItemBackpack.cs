using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Inventories
{
    /// A backpack is an item that can increase your inventory size.
    [CreateAssetMenu( fileName = "backpack item", menuName = "DwarfMiningGame/Item (Backpack)", order = 320 )]
    public class ItemBackpack : ItemEquippable
    {
        [field: SerializeField]
        public float MaxCapacity { get; set; }

        public override void OnEquip( Inventory i )
        {
            throw new System.NotImplementedException();
        }
#warning TODO - Make functional. Probably through ItemEquippable subclass with OnEquip() method.
    }
}