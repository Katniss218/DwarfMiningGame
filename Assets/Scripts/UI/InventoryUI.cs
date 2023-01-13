using DwarfMiningGame.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.UI
{
    public class InventoryUI : MonoBehaviour
    {
        // a grid with items.

        [field: SerializeField]
        public Inventory Inventory { get; set; }

        [SerializeField]
        RectTransform _list;

        Dictionary<Item, InventoryItemUI> _uis = new Dictionary<Item, InventoryItemUI>();

        void Awake()
        {
            this.Inventory.OnAdd += this.OnAdd;
            this.Inventory.OnRemove += this.OnRemove;
        }

        public void OnAdd( (Item item, int amt) e )
        {
            if( _uis.TryGetValue( e.item, out InventoryItemUI ui ) )
            {
                ui.AddAmount( e.amt );
            }
            else
            {
                ui = InventoryItemUI.Create( _list, e.item, e.amt );
                _uis.Add( e.item, ui );
            }
        }

        public void OnRemove( (Item item, int amt) e )
        {
            if( _uis.TryGetValue( e.item, out InventoryItemUI ui ) )
            {
                int left = ui.RemoveAmount( e.amt );
                if( left <= 0 )
                {
                    Destroy( ui.gameObject );
                    _uis.Remove( e.item );
                }
            }
            else
            {
                Debug.LogError( $"Inventory desync, item {e.item} wasn't added to the inventory UI." );
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