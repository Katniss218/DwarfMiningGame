using DwarfMiningGame.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityPlus.AssetManagement;

namespace DwarfMiningGame.UI
{
    /// <summary>
    /// Controls the display of an equipped item.
    /// </summary>
    public class InventoryEquipmentUI : MonoBehaviour
    {
        public const float SIZE = 70.0f;

        [SerializeField]
        Image _icon;

        public void SetSlot( Inventory.ItemSlot slot )
        {
            if( slot == null || slot.Item == null )
            {
                _icon.color = new Color( 0, 0, 0, 0 );
                return;
            }

            _icon.color = new Color( 1, 1, 1, 1 );
            _icon.sprite = slot.Item?.Icon;
        }

        /// <param name="slot">Optionally, provide a slot to set by default.</param>
        public static InventoryEquipmentUI Create( RectTransform invList, Action onClick, Inventory.ItemSlot slot = null )
        {
            GameObject rootGO = UIHelper.UI( invList, "item_ui", Vector2.zero, Vector2.zero, new Vector2( SIZE, SIZE ) );
            UIHelper.MakeRaycastTarget( rootGO );

            InventoryEquipmentUI ui = rootGO.AddComponent<InventoryEquipmentUI>();

            GameObject backgroundGO = UIHelper.UIFill( rootGO.transform, "background", 0, 0, 0, 0 );
            UIHelper.MakeForeground( backgroundGO );

            GameObject iconGO = UIHelper.UI( rootGO.transform, "item_icon", new Vector2( 0.5f, 0.5f ), Vector2.zero, new Vector2( 50, 50 ) );

            Image itemIcon = iconGO.AddComponent<Image>();
            itemIcon.raycastTarget = false;
            ui._icon = itemIcon;

            LeftClickAction aui = ui.gameObject.AddComponent<LeftClickAction>();
            aui.OnClick += onClick;

            ui.SetSlot( slot );

            return ui;
        }
    }
}