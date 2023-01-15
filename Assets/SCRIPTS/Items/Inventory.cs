using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Items
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
        // list of items, each contributes to a count.
        List<ItemSlot> _items = new List<ItemSlot>();

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

        public virtual int GetCount( Item item )
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

        /// Returns the space occupied by a specific item.
        public virtual float GetSize( Item item )
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

        public virtual float GetSize()
        {
            float acc = 0;
            foreach( ItemSlot stack in _items )
            {
                acc += stack.Amount * stack.Item.Size;
            }
            return acc;
        }

        /// Returns the space left.
        public virtual float GetSpaceLeft()
        {
            return MaxCapacity - GetSize();
        }

        /// <summary>
        /// Tries to add a specified amount of the specified item to the inventory. Only part will be added if the full amount can't fit in the inventory.
        /// </summary>
        /// <returns>How many items were actually added to the inventory.</returns>
        public virtual int TryAdd( Item item, int amount )
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
        public virtual int TryRemove( Item item, int amount )
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
                    OnRemove?.Invoke( (item, amountRemoved) );
                    OnAfterSlotChanged?.Invoke( slot );
                    if( slot.Amount <= 0 )
                    {
                        _items.Remove( slot );
                        OnSlotRemoved?.Invoke( slot );
                    }
                    return amountRemoved;
                }
            }
            return 0;
        }
    }
}