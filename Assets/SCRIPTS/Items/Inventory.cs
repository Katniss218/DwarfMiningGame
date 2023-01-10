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
            public int Amt { get; set; }

            public ItemStack( Item item, int amt )
            {
                this.Item = item;
                this.Amt = amt;
            }
        }

        [field: SerializeField]
        public float MaxCapacity { get; set; }

        [SerializeField]
        // list of items, each contributes to a count.
        List<ItemStack> _items = new List<ItemStack>();

        public virtual int GetCount( Item item )
        {
            int acc = 0;
            foreach( ItemStack stack in _items )
            {
                if( stack.Item == item )
                {
                    acc += stack.Amt;
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
                    acc += stack.Amt * stack.Item.Size;
                }
            }
            return acc;
        }

        public virtual float GetSize()
        {
            float acc = 0;
            foreach( ItemStack stack in _items )
            {
                acc += stack.Amt * stack.Item.Size;
            }
            return acc;
        }

        /// Returns the space left.
        public virtual float GetSpaceLeft()
        {
            return MaxCapacity - GetSize();
        }

        /// Returns how many items can be added.
        protected virtual int GetAmountAdded( Item item, int amt )
        {
            float sizeAdded = item.Size * amt;
            float sizeLeft = GetSpaceLeft();
            if( sizeAdded < sizeLeft )
            {
                sizeAdded = sizeLeft;
            }

            int amountAdded = Mathf.FloorToInt( sizeAdded / item.Size );

            return amountAdded;
        }

        /// returns amount added.
        public virtual int Add( Item item, int amt )
        {
            int amountAdded = GetAmountAdded( item, amt );
            if( amountAdded == 0 )
            {
                return 0;
            }

            foreach( ItemStack stack in _items )
            {
                if( stack.Item.ID == item.ID )
                {
                    stack.Amt += amountAdded;
                    return amountAdded;
                }
            }

            _items.Add( new ItemStack( item, amountAdded ) );
            return amountAdded;
        }

        /// returns amount removed.
        public virtual int Remove( Item item, int amt )
        {
            foreach( ItemStack stack in _items )
            {
                if( stack.Item.ID == item.ID )
                {
                    int amtRemoved = amt;
                    if( stack.Amt < amtRemoved )
                    {
                        amtRemoved = stack.Amt;
                    }

                    stack.Amt -= amtRemoved;
                    if( stack.Amt <= 0 )
                    {
                        _items.Remove( stack );
                    }
                    return amtRemoved;
                }
            }
            return 0;
        }
    }
}