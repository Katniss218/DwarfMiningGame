using DwarfMiningGame.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DwarfMiningGame.UI
{
    /// <summary>
    /// Controls the display of an item contained in an inventory.
    /// </summary>
    public class InventoryItemUI : MonoBehaviour
    {
        [SerializeField]
        Image _icon;

        [SerializeField]
        TMPro.TextMeshProUGUI _amountText;

        public void SetAmount( float amount )
        {
            _amountText.text = $"{amount}";
        }

        public void SetItem( Item item, int amount )
        {
            if( item == null )
            {
                _icon.color = new Color( 0, 0, 0, 0 );
                return;
            }

            _icon.color = new Color( 1, 1, 1, 1 );
            _icon.sprite = item.Icon;
            SetAmount( amount );
        }

        public static InventoryItemUI Create( RectTransform invList, Inventory.ItemSlot slot )
        {
            GameObject go = UIHelper.UI( invList, "equipment item UI", new Vector2( 0, 1 ), new Vector2( 0, 1 ), Vector2.zero, new Vector2( 100, 36 ) );
            
            InventoryItemUI ui = go.AddComponent<InventoryItemUI>();


            GameObject goI = UIHelper.UI( go.transform, "icon", new Vector2( 0.0f, 0.5f ), new Vector2( 0.0f, 0.0f ), new Vector2( 36.0f, 36.0f ) );

            Image img = goI.AddComponent<Image>();
            img.raycastTarget = false;
            ui._icon = img;

            GameObject goT = UIHelper.UI( go.transform, "icon", new Vector2( 1.0f, 0.5f ), new Vector2( 0.0f, 0.0f ), new Vector2( 100.0f, 36.0f ) );

            TMPro.TextMeshProUGUI text = UIHelper.AddText( goT, "<placeholder>", TMPro.HorizontalAlignmentOptions.Right );
            ui._amountText = text;
            
            ui.SetItem( slot.Item, slot.Amount );

            return ui;
        }
    }
}