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
                OnAfterEquipmentChanged( SLOT_MAINHAND, value );
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
                OnAfterEquipmentChanged( SLOT_OFFHAND, value );
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
                OnAfterEquipmentChanged( SLOT_BACKPACK, value );
            }
        }

        /*
        /// Bag changes the size of your inventory.
        // when changing the bag, if you unequip it, the capacity changes, but items are not dropped.
        public ItemSlot Bag
        {
            get => _equipment[0];
            set
            {
                float capacityWithoutOriginalBag = MaxCapacity;
                if( _equipment[0] != null )
                {
                    capacityWithoutOriginalBag -= ((ItemBag)_equipment[0].Item).MaxCapacity;
                }

                MaxCapacity = capacityWithoutOriginalBag;
                if( value != null )
                {
                    MaxCapacity += ((ItemBag)value.Item).MaxCapacity;
                }

                _equipment[0] = value;
            }
        }
        */
        /// <summary>
        /// buys an item and adds it to the inventory. enough money must be in the inventory. otherwise, only part will be bought.
        /// </summary>
        /// <returns>returns how many were bought.</returns>
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
        /// sells an item from the inventory and adds it to the money. enough items must be in the inventory. otherwise only part will be sold.
        /// </summary>
        /// <returns>returns how many were sold.</returns>
        public int TrySellItem( Item item, int amount )
        {
            int amountSold = TryRemove( item, amount );
            this.Money += item.Value * Item.SellValueMultiplier * amountSold;
            return amountSold;
        }
    }
}