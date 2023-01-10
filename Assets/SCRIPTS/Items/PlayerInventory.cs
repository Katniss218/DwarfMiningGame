using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.Items
{
    public class PlayerInventory : Inventory
    {
        [SerializeField]
        float _maxCapacityEmpty;

        /// Pickaxe lets you mine stuff.
        public PickaxeItem Pickaxe { get; set; }

        private BagItem _bag;

        /// Bag changes the size of your inventory.
        // when changing the bag, if you unequip it, the capacity changes, but items are not dropped.
        public BagItem Bag
        {
            get
            {
                return _bag;
            }
            set
            {
                _bag = value;
                MaxCapacity = _maxCapacityEmpty + value?.MaxCapacity ?? 0;
            }
        }
    }

}
