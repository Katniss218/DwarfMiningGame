using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.Inventories
{
    /// <summary>
    /// Represents an equippable item.
    /// </summary>
    public class Equipment : MonoBehaviour
    {
        /// <summary>
        /// A specific equipment slot (head, mainhand, backpack, etc).
        /// </summary>
        public class EquipmentSlot
        {
            /// Points to the slot that's equipped.
            public Inventory.ItemSlot CurrentItem { get; set; }
            /// Checks if the slot can be equipped.
            public Func<Inventory.ItemSlot, bool> CanEquip { get; set; }

            public Equipment Equipment { get; set; }

#warning TODO - move the event here and also tie the UI capacity bar to it.

            /// Equips a specified item (if possible).
            public void Equip( Inventory.ItemSlot slot )
            {
                // Validate that we can equip in the first place.
                if( !CanEquip( slot ) || !(slot.Item is IItemEquippable) )
                {
                    throw new InvalidOperationException( "Can't equip item" );
                }
                // Unequip current before equipping new.
                if( CurrentItem != null )
                {
                    Unequip();
                }
                // Equip new.
                CurrentItem = slot;
                IItemEquippable ie = (IItemEquippable)CurrentItem.Item;
                ie.OnEquip( Equipment );
            }


            /// Unequips the current item.
            public void Unequip()
            {
                // No need to validate or anything, since it's not possible to equip a bad item.
                IItemEquippable ie = (IItemEquippable)CurrentItem.Item;
                CurrentItem = null;
                ie.OnUnequip( Equipment );
            }
        }


        public EquipmentSlot[] Slots { get; set; }
    }
}