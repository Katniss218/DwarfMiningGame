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
    /// The user interface for a <see cref="ShopBehaviour"/>.
    /// </summary>
    public class ShopUI : MonoBehaviour
    {
        // #############################

        // Design / Assumptions:
        // - A scrollable vertical list of entries.
        //   - Each entry displays the item, amount, price, and how much remaining.

        // - Click on an item to buy it.

        // #############################

        public ShopBehaviour Shop { get; private set; }

        // holds the action that happens when the recipe entry is clicked. It's a way to pass the inventory through.
        Action<(Shop.Offer offer, int amount)> _onClickEntry;

        RectTransform _list;
        Dictionary<Shop.Offer, ShopEntryUI> _entryUIs = new Dictionary<Shop.Offer, ShopEntryUI>();

        /// <summary>
        /// (Re)Binds the UI to a specific sell shop behaviour.
        /// </summary>
        public void SetShop( ShopBehaviour shop )
        {
            Shop = shop;
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
            foreach( var offer in Shop.Shop.BuyOffers )
            {
                ShopEntryUI ui = ShopEntryUI.Create( _list, offer, _onClickEntry );
                _entryUIs.Add( offer, ui );
            }
        }

        public static ShopUI Create( RectTransform mainCanvas, ShopBehaviour shop, Action<(Shop.Offer offer, int amount)> onClickItem )
        {
            GameObject rootGO = UIHelper.UI( mainCanvas.transform, "sell shop", new Vector2( 0.5f, 0.5f ), Vector2.zero, new Vector2( 300, 280 ) );

            GameObject content = UIHelper.AddScrollRect( rootGO, false, true );
            UIHelper.MakeBackground( rootGO );
            UIHelper.MakeVerticalLayoutGroup( content, 5, 5, true );

            ShopUI ui = rootGO.AddComponent<ShopUI>();
            ui._list = content.GetComponent<RectTransform>();
            ui._onClickEntry = onClickItem;
            ui.SetShop( shop );

            return ui;
        }
    }
}
