using DwarfMiningGame.Inventories;
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
        public const float WIDTH = 75.0f;

        public const float HEIGHT = 36.0f;

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

        public static InventoryItemUI Create( RectTransform list, Item item, int amount )
        {
            GameObject go = UIHelper.UI( list, "equipment item UI", new Vector2( 0.0f, 1.0f ), new Vector2( 0.0f, 1.0f ), Vector2.zero, new Vector2( WIDTH, HEIGHT ) );

            InventoryItemUI ui = go.AddComponent<InventoryItemUI>();

            GameObject goI = UIHelper.UI( go.transform, "icon", new Vector2( 0.0f, 0.5f ), new Vector2( 0.0f, 0.0f ), new Vector2( HEIGHT, HEIGHT ) );

            Image img = goI.AddComponent<Image>();
            img.raycastTarget = false;
            ui._icon = img;

            GameObject goT = UIHelper.UIFill( go.transform, "icon", 0, 0, 0, 0 );

            TMPro.TextMeshProUGUI text = UIHelper.MakeText( goT, "<placeholder>", TMPro.HorizontalAlignmentOptions.Right );
            ui._amountText = text;

            ui.SetItem( item, amount );

            return ui;
        }
    }
}