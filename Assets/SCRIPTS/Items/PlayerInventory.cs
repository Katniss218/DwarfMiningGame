using DwarfMiningGame.UI;
using System;
using UnityEngine;

namespace DwarfMiningGame.Items
{
    public class PlayerInventory : Inventory
    {
        // equipment is a list, you click on an item, it gives you a popup where you can select an item to equip.
        [SerializeField]
        private float _money = 0;
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

        ItemSlot[] _equipment = new ItemSlot[] { null };

        /// Pickaxe lets you mine stuff.
        public ItemSlot Hand
        {
            get => _equipment[0];
            set
            {
                _equipment[0] = value;
                OnAfterEquipmentChanged( 0, value );
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