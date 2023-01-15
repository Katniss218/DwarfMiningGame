using DwarfMiningGame.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DwarfMiningGame.UI
{
    public class InventoryUI : MonoBehaviour
    {
        // a grid with items.

        [field: SerializeField]
        public Inventory Inventory { get; set; }

        [SerializeField]
        RectTransform _list;

        Dictionary<Inventory.ItemSlot, InventoryItemUI> _uis = new Dictionary<Inventory.ItemSlot, InventoryItemUI>();

        protected virtual void Awake()
        {
            this.Inventory.OnAfterSlotChanged += this.OnAfterSlotChanged;
            this.Inventory.OnSlotAdded += this.OnSlotAdded;
            this.Inventory.OnSlotRemoved += this.OnSlotRemoved;
        }

        void OnAfterSlotChanged( Inventory.ItemSlot slot )
        {
            if( _uis.TryGetValue( slot, out InventoryItemUI ui ) )
            {
                ui.SetAmount( slot.Amount );
            }
            else
            {
                Debug.LogError( $"Inventory desync, slot {slot} wasn't added to the inventory UI." );
            }
        }

        void OnSlotAdded( Inventory.ItemSlot slot )
        {
            if( !_uis.ContainsKey( slot ) )
            {
                InventoryItemUI ui = InventoryItemUI.Create( _list, slot );

#warning TODO - remove after a proper shop is implemented.
                Image raycastImage = ui.gameObject.AddComponent<Image>();
                raycastImage.raycastTarget = true;
                raycastImage.color = new Color( 0.0f, 0.0f, 0.0f, 0.0f ); // transparent.

                LeftClickAction aui = ui.gameObject.AddComponent<LeftClickAction>();

                var slotTemp = slot;
                aui.OnClick += () => ((PlayerInventory)Inventory).TrySellItem( slotTemp.Item, slotTemp.Amount );

                _uis.Add( slot, ui );
            }
        }

        void OnSlotRemoved( Inventory.ItemSlot slot )
        {
            if( _uis.TryGetValue( slot, out InventoryItemUI ui ) )
            {
                Destroy( ui.gameObject );
                _uis.Remove( slot );
            }
        }

        public static void Create( RectTransform panel )
        {
            throw new NotImplementedException();

            /*
            GameObject go = new GameObject( "Inventory UI" );
            RectTransform rt = go.AddComponent<RectTransform>();
            rt.SetParent( panel );

            InventoryUI ui = go.AddComponent<InventoryUI>();
            */
            //ui._list = list;

            // inv_ui
            // - scroll
            // - - scroll_rect
            // - - - contents
            // - scrollbar_vertical
            // - - sliding_area
            // - - - handle
            // - scrollbar_horizontal
            // - - sliding_area
            // - - - handle
        }

    }
}