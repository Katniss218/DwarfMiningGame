using DwarfMiningGame.UI;
using System;
using UnityEngine;

namespace DwarfMiningGame.Inventories
{
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

        // equipment is a list, you click on an item, it gives you a popup where you can select an item to equip.
        ItemSlot[] _equipment = new ItemSlot[] { null, null, null };

        const int SLOT_MAINHAND = 0;
        const int SLOT_OFFHAND = 1;
        const int SLOT_BACKPACK = 2;

        /// <summary>
        /// Gets or sets the item in the primary (main) hand. Shorthand for the equipment slot.
        /// </summary>
        public ItemSlot MainHand
        {
            get => _equipment[SLOT_MAINHAND];
            set
            {
                _equipment[SLOT_MAINHAND] = value;
                OnAfterEquipmentChanged?.Invoke( SLOT_MAINHAND, value );
            }
        }

        /// <summary>
        /// Gets or sets the item in the secondary (off) hand. Shorthand for the equipment slot.
        /// </summary>
        public ItemSlot OffHand
        {
            get => _equipment[SLOT_OFFHAND];
            set
            {
                _equipment[SLOT_OFFHAND] = value;
                OnAfterEquipmentChanged?.Invoke( SLOT_OFFHAND, value );
            }
        }

        /// <summary>
        /// Gets or sets the backpack item. Shorthand for the equipment slot.
        /// </summary>
        public ItemSlot Backpack
        {
            get => _equipment[SLOT_BACKPACK];
            set
            {
                _equipment[SLOT_BACKPACK] = value;
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
            for( int i = 0; i < _equipment.Length; i++ )
            {
                if( slot == _equipment[i] )
                {
                    _equipment[i] = null;
                    OnAfterEquipmentChanged?.Invoke( i, null );
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