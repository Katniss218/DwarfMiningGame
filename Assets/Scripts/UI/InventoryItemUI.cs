using DwarfMiningGame.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DwarfMiningGame.UI
{
    public class InventoryItemUI : MonoBehaviour
    {
        [SerializeField]
        Image _icon;

        [SerializeField]
        TMPro.TextMeshProUGUI _text;

        public void SetAmount( float amount )
        {
            _text.text = $"{amount}";
        }

        public void SetItem( Item item, int amount )
        {
            _icon.sprite = item.Icon;
            SetAmount( amount );
        }

        public static InventoryItemUI Create( RectTransform invList, Inventory.ItemSlot slot )
        {
            GameObject go = new GameObject( "Item UI" );
            RectTransform rt = go.AddComponent<RectTransform>();
            rt.SetParent( invList );
            rt.sizeDelta = new Vector2( 100.0f, 36.0f );
            // sizing handled by the list's grid layout.

            InventoryItemUI ui = go.AddComponent<InventoryItemUI>();


            GameObject goI = new GameObject( "Icon" );
            RectTransform rtI = goI.AddComponent<RectTransform>();
            rtI.SetParent( rt );
            rtI.anchorMin = new Vector2( 0.0f, 0.5f );
            rtI.anchorMax = new Vector2( 0.0f, 0.5f );
            rtI.pivot = new Vector2( 0.0f, 0.5f );
            rtI.anchoredPosition = new Vector2( 0.0f, 0.0f );
            rtI.sizeDelta = new Vector2( 36.0f, 36.0f );

            Image img = goI.AddComponent<Image>();
            img.raycastTarget = false;
            ui._icon = img;


            GameObject goT = new GameObject( "Text" );
            RectTransform rtT = goT.AddComponent<RectTransform>();
            rtT.SetParent( rt );
            rtT.anchorMin = new Vector2( 1.0f, 0.5f );
            rtT.anchorMax = new Vector2( 1.0f, 0.5f );
            rtT.pivot = new Vector2( 1.0f, 0.5f );
            rtT.anchoredPosition = new Vector2( 0.0f, 0.0f );
            rtT.sizeDelta = new Vector2( 75.0f, 36.0f );

            TMPro.TextMeshProUGUI text = goT.AddComponent<TMPro.TextMeshProUGUI>();
            text.fontSize = 16.0f;
            text.raycastTarget = false;
            text.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Right;
            text.verticalAlignment = TMPro.VerticalAlignmentOptions.Bottom;
            text.overflowMode = TMPro.TextOverflowModes.Overflow;
            text.enableWordWrapping = false;
            ui._text = text;

            ui.SetItem( slot.Item, slot.Amount );

            return ui;
        }
    }
}