using DwarfMiningGame.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityPlus.AssetManagement;

namespace DwarfMiningGame.UI
{
    /// <summary>
    /// A class that lets you select an item from the inventory.
    /// </summary>
    public class InventorySelectorContextMenu : ContextMenu
    {
        Inventory _inventory;
        Func<Inventory.ItemSlot, bool> _canEquip;
        Action<Inventory.ItemSlot> _onSelect;

        bool _closeOnSelect = false;

        RectTransform _list;

        Dictionary<Inventory.ItemSlot, ItemUI> _activeSlots = new Dictionary<Inventory.ItemSlot, ItemUI>();

        void OnAfterSlotChanged( Inventory.ItemSlot slot )
        {
            // the selector might pass the same slot based on the conditions of the slot,
            // - so we need to check whether or not it's STILL available, and whether it's NOT available ANYMORE.

            if( _activeSlots.TryGetValue( slot, out ItemUI iui ) )
            {
                if( !_canEquip( slot ) )
                {
                    RemoveItem( slot );
                }
                else
                {
                    iui.SetAmount( slot.Amount );
                }
            }
            else
            {
                if( _canEquip( slot ) )
                {
                    AddItem( slot );
                }
            }
        }

        void OnSlotAdded( Inventory.ItemSlot slot )
        {
            if( _canEquip( slot ) )
            {
                AddItem( slot );
            }
        }

        void OnSlotRemoved( Inventory.ItemSlot slot )
        {
            if( _activeSlots.ContainsKey( slot ) )
            {
                RemoveItem( slot );
            }
        }

        void AddItem( Inventory.ItemSlot slot )
        {
            ItemUI iui = ItemUI.Create( _list, slot.Item, slot.Amount );
            UIHelper.MakeRaycastTarget( iui.gameObject );

            LeftClickAction c = iui.gameObject.AddComponent<LeftClickAction>();
            c.OnClick += () =>
            {
                _onSelect( slot );
                if( _closeOnSelect )
                {
                    this.Close();
                }
            };

            _activeSlots.Add( slot, iui );
        }

        void RemoveItem( Inventory.ItemSlot slot )
        {
            ItemUI iui = _activeSlots[slot];

            Destroy( iui.gameObject );
            _activeSlots.Remove( slot );
        }

        void OnDestroy()
        {
            if( _inventory != null )
            {
                _inventory.OnAfterSlotChanged -= this.OnAfterSlotChanged;
                _inventory.OnSlotAdded -= this.OnSlotAdded;
                _inventory.OnSlotRemoved -= this.OnSlotRemoved;
            }
        }

        /// <summary>
        /// Makes a window that equips an item to an equipment slot.
        /// </summary>
        public static InventorySelectorContextMenu Create( RectTransform parent, Inventory inventory, bool closeOnSelect, Func<Inventory.ItemSlot, bool> canEquipItem, Action<Inventory.ItemSlot> onSelect )
        {
            // a 1D vertical list of all the items that can be equipped in the selected slot.
            // 

            GameObject gameObject = UIHelper.UI( parent, "Inventory Item Selector Window", new Vector2( 0.0f, 1.0f ), new Vector2( 455.0f, -85.0f ), new Vector2( 100.0f, 280.0f ) );

            InventorySelectorContextMenu isw = gameObject.AddComponent<InventorySelectorContextMenu>();
            isw._canEquip = canEquipItem;
            isw._onSelect = onSelect;
            isw._closeOnSelect = closeOnSelect;
            isw._inventory = inventory;
            isw._inventory.OnAfterSlotChanged += isw.OnAfterSlotChanged;
            isw._inventory.OnSlotAdded += isw.OnSlotAdded;
            isw._inventory.OnSlotRemoved += isw.OnSlotRemoved;

            GameObject items = UIHelper.UIFill( gameObject.transform, "items" );
            UIHelper.MakeForeground( items );

            GameObject content = UIHelper.AddScrollRect( items, false, true );

            UIHelper.MakeVerticalLayoutGroup( content, 5, 0, true );

            isw._list = (RectTransform)content.transform;

            foreach( var slot in inventory.GetItems() )
            {
                if( isw._canEquip( slot ) )
                {
                    isw.AddItem( slot );
                }
            }

            return isw;
        }
    }
}