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

        int _amount;

        private void SetText()
        {
            _text.text = $"{_amount}";
        }

        public int AddAmount( int amountAdded )
        {
            _amount += amountAdded;
            SetText();
            return _amount;
        }

        public int RemoveAmount( int amountRemoved )
        {
            _amount -= amountRemoved;
            SetText();
            return _amount;
        }

        public void SetItem( Item item, int amount )
        {
            _amount = amount;
            _icon.sprite = item.Icon;
            SetText();
        }

        public static InventoryItemUI Create( RectTransform invList, Item item, int amount )
        {
            GameObject go = new GameObject( "Item UI" );
            RectTransform rt = go.AddComponent<RectTransform>();
            rt.SetParent( invList );
            // sizing handled by the list's grid layout.

            InventoryItemUI ui = go.AddComponent<InventoryItemUI>();


            GameObject goI = new GameObject( "Icon" );
            RectTransform rtI = goI.AddComponent<RectTransform>();
            rtI.SetParent( rt );
            rtI.anchorMin = new Vector2( 0.0f, 0.5f );
            rtI.anchorMax = new Vector2( 0.0f, 0.5f );
            rtI.pivot = new Vector2( 0.0f, 0.5f );
            rtI.anchoredPosition = new Vector2( 0.0f, 0.0f );
            rtI.sizeDelta = new Vector2( 24.0f, 24.0f );

            Image img = goI.AddComponent<Image>();
            img.raycastTarget = false;
            ui._icon = img;


            GameObject goT = new GameObject( "Icon" );
            RectTransform rtT = goT.AddComponent<RectTransform>();
            rtT.SetParent( rt );
            rtT.anchorMin = new Vector2( 1.0f, 0.5f );
            rtT.anchorMax = new Vector2( 1.0f, 0.5f );
            rtT.pivot = new Vector2( 1.0f, 0.5f );
            rtT.anchoredPosition = new Vector2( 0.0f, 0.0f );
            rtT.sizeDelta = new Vector2( 48.0f, 24.0f );

            TMPro.TextMeshProUGUI text = goT.AddComponent<TMPro.TextMeshProUGUI>();
            text.raycastTarget = false;
            text.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Right;
            text.overflowMode = TMPro.TextOverflowModes.Overflow;
            text.enableWordWrapping = false;
            ui._text = text;

            ui.SetItem( item, amount );

            return ui;
        }
    }
}