using DwarfMiningGame.Inventories;
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
    /// A single entry in the SellShopUI.
    /// </summary>
    public class SellShopEntryUI : MonoBehaviour, IPointerClickHandler
    {
        // sell the item on click.

        Inventory.ItemSlot _slot;

        Action<(Item item, int amount)> _onClick;

        ItemUI _inventoryItemUI;

        TMPro.TextMeshProUGUI _priceText;

        public void OnPointerClick( PointerEventData eventData )
        {
            if( eventData.button == PointerEventData.InputButton.Left )
            {
                _onClick?.Invoke( (_slot.Item, 1) );
            }
        }

        public static SellShopEntryUI Create( RectTransform parent, Inventory.ItemSlot slot, Action<(Item item, int amount)> onClick )
        {
            // SellShopUI
            // - List of SellShopEntryUI (full width)
            // - - Item
            // - - Sell price

            GameObject root = UIHelper.UI( parent, "container", new Vector2( 0, 0 ), new Vector2( 1, 0 ), new Vector2( parent.sizeDelta.x, ItemUI.HEIGHT ) );

            UIHelper.MakeRaycastTarget( root );

            SellShopEntryUI ui = root.AddComponent<SellShopEntryUI>();
            ui._slot = slot;
            ui._onClick = onClick;
            ui._inventoryItemUI = ItemUI.Create( (RectTransform)root.transform, slot.Item, slot.Amount );

            GameObject priceText = UIHelper.UIFill( root.transform, "price text", 0, 0, 0, 0 );
            TMPro.TextMeshProUGUI text = UIHelper.MakeText( priceText, "<price>", TMPro.HorizontalAlignmentOptions.Right );
            ui._priceText = text;

            ui._priceText.text = $"{PlayerInventoryUI.FormatMoney( slot.Item.SellValue * slot.Amount )}\t(1x {PlayerInventoryUI.FormatMoney( slot.Item.SellValue )})";

            return ui;
        }
    }
}