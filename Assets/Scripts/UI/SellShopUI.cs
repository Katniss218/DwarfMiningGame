using DwarfMiningGame.Inventories;
using DwarfMiningGame.Shops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.UI
{
    /// <summary>
    /// The user interface for a <see cref="SellShopBehaviour"/>.
    /// </summary>
    public class SellShopUI : MonoBehaviour
    {
        // #############################

        // Design / Assumptions:
        // - A scrollable vertical list of items in the inventory.
        //   - Each item displays the item and for how much money it sells.

        // - Click on an item to sell it.

        // #############################

        public SellShopBehaviour SellShop { get; private set; }

        // holds the action that happens when the recipe entry is clicked. It's a way to pass the inventory through.
        Action<(Item item, int amount)> _onClickEntry;
        Func<Inventory.ItemSlot[]> _getSlots;

        RectTransform _list;
        Dictionary<Inventory.ItemSlot, SellShopEntryUI> _entryUIs = new Dictionary<Inventory.ItemSlot, SellShopEntryUI>();

        /// <summary>
        /// (Re)Binds the UI to a specific sell shop behaviour.
        /// </summary>
        public void SetSellShop( SellShopBehaviour sellShop )
        {
            SellShop = sellShop;
            Redraw();
        }

        public void Redraw()
        {
            DeleteEntries();
            AddEntries();
        }

        private void DeleteEntries()
        {
            foreach( var ui in _entryUIs.Values )
            {
                Destroy( ui.gameObject );
            }
            _entryUIs.Clear();
        }

        private void AddEntries()
        {
            Inventory.ItemSlot[] slots = _getSlots();

            foreach( var slot in slots )
            {
                SellShopEntryUI ui = SellShopEntryUI.Create( _list, slot, _onClickEntry );
                _entryUIs.Add( slot, ui );
            }
        }

        public static SellShopUI Create( RectTransform mainCanvas, SellShopBehaviour sellShop, Func<Inventory.ItemSlot[]> getSlots, Action<(Item item, int amount)> onClickItem )
        {
            GameObject rootGO = UIHelper.UI( mainCanvas.transform, "sell shop", new Vector2( 0.5f, 0.5f ), Vector2.zero, new Vector2( 300, 280 ) );

            GameObject content = UIHelper.AddScrollRect( rootGO, false, true );
            UIHelper.MakeBackground( rootGO );
            UIHelper.MakeVerticalLayoutGroup( content, 5, 5, true );

            SellShopUI ui = rootGO.AddComponent<SellShopUI>();
            ui._list = content.GetComponent<RectTransform>();
            ui._getSlots = getSlots;
            ui._onClickEntry = onClickItem;
            ui.SetSellShop( sellShop );

            return ui;
        }
    }
}
