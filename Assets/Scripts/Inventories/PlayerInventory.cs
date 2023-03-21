using DwarfMiningGame.UI;
using System;
using UnityEngine;

namespace DwarfMiningGame.Inventories
{
    [RequireComponent( typeof( Equipment ) )]
    public class PlayerInventory : Inventory
    {
        [SerializeField]
        private float _money = 0;
        /// <summary>
        /// Gets or sets the money of this inventory.
        /// </summary>
        public float Money
        {
            get
            {
                return _money;
            }
            set
            {
                float oldMoney = _money;
                _money = value;
                OnMoneyChangedDelta?.Invoke( _money - oldMoney );
                OnAfterMoneyChanged?.Invoke( _money );
            }
        }

        /// <summary>
        /// OnMoneyChanged( deltaMoney )
        /// </summary>
        public Action<float> OnMoneyChangedDelta;

        /// <summary>
        /// OnMoneyChanged( deltaMoney )
        /// </summary>
        public Action<float> OnAfterMoneyChanged;

        /// <summary>
        /// OnAfterEquipmentChanged( slotIndex, newItem )
        /// </summary>
        public Action<int, ItemSlot> OnAfterEquipmentChanged;

        const int SLOT_MAINHAND = 0;
        const int SLOT_OFFHAND = 1;
        const int SLOT_BACKPACK = 2;

        // The equipment slots of the player's inventory and their corresponding validator functions.
        Equipment _equipment;

        void Awake()
        {
            _equipment = this.GetComponent<Equipment>();

            _equipment.Slots = new Equipment.EquipmentSlot[]
            {
            new Equipment.EquipmentSlot() // mainhand
            { CurrentItem = null, CanEquip = (i) => i.Item is ItemPickaxe, Equipment = _equipment },
            new Equipment.EquipmentSlot() // offhand
            { CurrentItem = null, CanEquip = (i) => true, Equipment = _equipment },
            new Equipment.EquipmentSlot() // backpack
            { CurrentItem = null, CanEquip = (i) => i.Item is ItemBackpack, Equipment = _equipment }
            };
        }


        /// <summary>
        /// Gets or sets the item in the primary (main) hand. Shorthand for the equipment slot.
        /// </summary>
        public ItemSlot MainHand
        {
            get => _equipment.Slots[SLOT_MAINHAND].CurrentItem;
            set
            {
                _equipment.Slots[SLOT_MAINHAND].Equip( value );
                OnAfterEquipmentChanged?.Invoke( SLOT_MAINHAND, value );
            }
        }

        /// <summary>
        /// Gets or sets the item in the secondary (off) hand. Shorthand for the equipment slot.
        /// </summary>
        public ItemSlot OffHand
        {
            get => _equipment.Slots[SLOT_OFFHAND].CurrentItem;
            set
            {
                _equipment.Slots[SLOT_OFFHAND].Equip( value );
                OnAfterEquipmentChanged?.Invoke( SLOT_OFFHAND, value );
            }
        }

        /// <summary>
        /// Gets or sets the backpack item. Shorthand for the equipment slot.
        /// </summary>
        public ItemSlot Backpack
        {
            get => _equipment.Slots[SLOT_BACKPACK].CurrentItem;
            set
            {
                _equipment.Slots[SLOT_BACKPACK].Equip( value );
                OnAfterEquipmentChanged?.Invoke( SLOT_BACKPACK, value );
            }
        }

        public static bool CanEquipMainhand( ItemSlot item )
        {
            return item.Item is ItemPickaxe;
        }

        public static bool CanEquipOffhand( ItemSlot item )
        {
            return true;
        }

        public static bool CanEquipBackpack( ItemSlot item )
        {
            return item.Item is ItemBackpack;
        }

        protected override void SlotRemoved( ItemSlot slot )
        {
            base.SlotRemoved( slot );

            // If someone removed the item referenced as equipment - unreference it.
            for( int i = 0; i < _equipment.Slots.Length; i++ )
            {
                if( _equipment.Slots[i].CurrentItem == slot )
                {
                    _equipment.Slots[i].Unequip();
                }
            }
        }

        /// <summary>
        /// Buys an item and adds it to the inventory. Enough money must be in the inventory, otherwise, only part will be bought.
        /// </summary>
        /// <returns>How many were bought.</returns>
        public int TryBuyItem( Item item, int amount )
        {
            // clamp how many we can buy to the available money.
            int amountBought = amount;
            if( item.Value * amount > Money )
            {
                amountBought = Mathf.FloorToInt( Money / item.Value );
            }

            // get how many items actually fit.
            amountBought = this.TryAdd( item, amountBought );
            this.Money -= item.Value * amountBought;

            return amountBought;
        }

        /// <summary>
        /// Sells an item from the inventory and adds it to the money. Enough items must be in the inventory, otherwise only part will be sold.
        /// </summary>
        /// <returns>How many were sold.</returns>
        public int TrySellItem( Item item, int amount )
        {
            int amountSold = TryRemove( item, amount );
            this.Money += item.SellValue * amountSold;
            return amountSold;
        }
    }
}