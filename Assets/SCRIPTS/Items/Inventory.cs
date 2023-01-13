using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Items
{
    public class Inventory : MonoBehaviour
    {
        [Serializable]
        public class ItemStack
        {
            [field: SerializeField]
            public Item Item { get; set; }
            [field: SerializeField]
            public int Amount { get; set; }

            public ItemStack( Item item, int amt )
            {
                this.Item = item;
                this.Amount = amt;
            }
        }

        [field: SerializeField]
        public float MaxCapacity { get; set; }

        [SerializeField]
        // list of items, each contributes to a count.
        List<ItemStack> _items = new List<ItemStack>();

        [SerializeField]
        public Action<(Item item, int amt)> OnAdd;

        [SerializeField]
        public Action<(Item item, int amt)> OnRemove;

        public virtual int GetCount( Item item )
        {
            int acc = 0;
            foreach( ItemStack stack in _items )
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
            foreach( ItemStack stack in _items )
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
            foreach( ItemStack stack in _items )
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

        /// returns amount added.
        public virtual int Add( Item item, int amount )
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

            foreach( ItemStack stack in _items )
            {
                if( stack.Item.ID == item.ID )
                {
                    stack.Amount += amountAdded;
                    OnAdd?.Invoke( (item, amountAdded) );
                    return amountAdded;
                }
            }

            OnAdd?.Invoke( (item, amountAdded) );
            _items.Add( new ItemStack( item, amountAdded ) );
            return amountAdded;
        }

        /// returns amount removed.
        public virtual int Remove( Item item, int amount )
        {
            foreach( ItemStack stack in _items )
            {
                if( stack.Item.ID == item.ID )
                {
                    int amountRemoved = amount;
                    if( stack.Amount < amountRemoved )
                    {
                        amountRemoved = stack.Amount;
                    }

                    stack.Amount -= amountRemoved;
                    if( stack.Amount <= 0 )
                    {
                        _items.Remove( stack );
                    }
                    OnRemove?.Invoke( (item, amountRemoved) );
                    return amountRemoved;
                }
            }
            return 0;
        }
    }
}