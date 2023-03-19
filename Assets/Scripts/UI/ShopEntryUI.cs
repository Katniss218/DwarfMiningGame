using DwarfMiningGame.Inventories;
using DwarfMiningGame.Shops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DwarfMiningGame.UI
{
    /// <summary>
    /// A single entry in the <see cref="ShopUI"/>.
    /// </summary>
    public class ShopEntryUI : MonoBehaviour, IPointerClickHandler
    {
        // sell the item on click.

        Shop.Offer _offer;

        Action<(Shop.Offer offer, int amount)> _onClick;

        ItemUI _inventoryItemUI;

        TMPro.TextMeshProUGUI _priceText;

        public void OnPointerClick( PointerEventData eventData )
        {
            if( eventData.button == PointerEventData.InputButton.Left )
            {
                _onClick?.Invoke( (_offer, 1) );
            }
        }

        public static ShopEntryUI Create( RectTransform parent, Shop.Offer offer, Action<(Shop.Offer offer, int amount)> onClick )
        {
            // SellShopUI
            // - List of SellShopEntryUI (full width)
            // - - Item
            // - - Sell price

            GameObject root = UIHelper.UI( parent, "container", new Vector2( 0, 0 ), new Vector2( 1, 0 ), new Vector2( parent.sizeDelta.x, ItemUI.HEIGHT ) );

            UIHelper.MakeRaycastTarget( root );

            ShopEntryUI ui = root.AddComponent<ShopEntryUI>();
            ui._offer = offer;
            ui._onClick = onClick;
            ui._inventoryItemUI = ItemUI.Create( (RectTransform)root.transform, offer.Item, offer.Amount );

            GameObject priceText = UIHelper.UIFill( root.transform, "price text", 0, 0, 0, 0 );
            TMPro.TextMeshProUGUI text = UIHelper.MakeText( priceText, "<price>", TMPro.HorizontalAlignmentOptions.Right );
            ui._priceText = text;

            ui._priceText.text = $"{PlayerInventoryUI.FormatMoney( offer.Item.Value * offer.Amount )}\t(1x {PlayerInventoryUI.FormatMoney( offer.Item.SellValue )})";

            return ui;
        }
    }
}