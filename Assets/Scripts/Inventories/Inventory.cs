using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Inventories
{
    public class Inventory : MonoBehaviour
    {
        [Serializable]
        public class ItemSlot
        {
            [field: SerializeField]
            public Item Item { get; set; }

            [field: SerializeField]
            public int Amount { get; set; }

            public ItemSlot( Item item, int amt )
            {
                this.Item = item;
                this.Amount = amt;
            }
        }

        [field: SerializeField]
        public float MaxCapacity { get; set; }

        [SerializeField]
        List<ItemSlot> _items = new List<ItemSlot>(); // list of items, each contributes to a count.

        [SerializeField]
        public Action<(Item item, int amt)> OnAdd;

        [SerializeField]
        public Action<(Item item, int amt)> OnRemove;

        /// <summary>
        /// Called
        /// </summary>
        [SerializeField]
        public Action<ItemSlot> OnAfterSlotChanged;

        /// <summary>
        /// Called before the OnSlotChanged.
        /// </summary>
        [SerializeField]
        public Action<ItemSlot> OnSlotAdded;

        /// <summary>
        /// Called after the OnSlotChanged.
        /// </summary>
        [SerializeField]
        public Action<ItemSlot> OnSlotRemoved;

        public ItemSlot[] GetItems()
        {
            return this._items.ToArray();
        }

        /// <summary>
        /// Returns the number of items of the specified type that currently are in the inventory.
        /// </summary>
        public int GetAmount( Item item )
        {
            int acc = 0;
            foreach( ItemSlot stack in _items )
            {
                if( stack.Item == item )
                {
                    acc += stack.Amount;
                }
            }
            return acc;
        }

        /// <summary>
        /// Returns the volume occupied by a specific type of item.
        /// </summary>
        public float GetSize( Item item )
        {
            float acc = 0;
            foreach( ItemSlot stack in _items )
            {
                if( stack.Item.ID == item.ID )
                {
                    acc += stack.Amount * stack.Item.Size;
                }
            }
            return acc;
        }

        /// <summary>
        /// Returns the total size of the inventory in volume units.
        /// </summary>
        /// <returns></returns>
        public float GetSize()
        {
            float acc = 0;
            foreach( ItemSlot stack in _items )
            {
                acc += stack.Amount * stack.Item.Size;
            }
            return acc;
        }

        /// <summary>
        /// Returns the space left in the inventory (in volume units).
        /// </summary>
        public float GetSpaceLeft()
        {
            return MaxCapacity - GetSize();
        }

        /// <summary>
        /// Returns the space left in the inventory (in units of amount of item).
        /// </summary>
        public int GetSpaceLeft( Item item )
        {
            return Mathf.FloorToInt(GetSpaceLeft() / item.Size);
        }

        /// <summary>
        /// Tries to add a specified amount of the specified item to the inventory. Only part will be added if the full amount can't fit in the inventory.
        /// </summary>
        /// <returns>How many items were actually added to the inventory.</returns>
        public int TryAdd( Item item, int amount )
        {
            float spaceLeft = GetSpaceLeft();
            int amountLeft = Mathf.FloorToInt( spaceLeft / item.Size );
            int amountAdded = amount;
            if( amountLeft < amountAdded )
            {
                amountAdded = amountLeft;
            }

            if( amountAdded <= 0 )
            {
                return 0;
            }

            foreach( ItemSlot slot in _items )
            {
                if( slot.Item.ID == item.ID )
                {
                    slot.Amount += amountAdded;
                    OnAdd?.Invoke( (item, amountAdded) );
                    OnAfterSlotChanged?.Invoke( slot );
                    return amountAdded;
                }
            }

            ItemSlot newSlot = new ItemSlot( item, amountAdded );
            _items.Add( newSlot );
            OnAdd?.Invoke( (item, amountAdded) );
            OnSlotAdded?.Invoke( newSlot );
            OnAfterSlotChanged?.Invoke( newSlot );
            return amountAdded;
        }

        /// <summary>
        /// Tries to remove a specified amount of the specified item from the inventory. Only part will be removed if there is not enough items present in the inventory.
        /// </summary>
        /// <returns>How many items were actually removed from the inventory.</returns>
        public int TryRemove( Item item, int amount )
        {
            foreach( ItemSlot slot in _items )
            {
                if( slot.Item.ID == item.ID )
                {
                    int amountRemoved = amount;
                    if( slot.Amount < amountRemoved )
                    {
                        amountRemoved = slot.Amount;
                    }

                    slot.Amount -= amountRemoved;
                    if( slot.Amount <= 0 ) // This would be an issue for iteration, if not for the fact that only one slot can contain the same item.
                    {
                        _items.Remove( slot );
                        OnRemove?.Invoke( (item, amountRemoved) ); // duplicate code because event calling order.
                        OnAfterSlotChanged?.Invoke( slot );
                        OnSlotRemoved?.Invoke( slot );
                    }
                    else
                    {
                        OnRemove?.Invoke( (item, amountRemoved) );
                        OnAfterSlotChanged?.Invoke( slot );
                    }
                    return amountRemoved;
                }
            }
            return 0;
        }
    }
}