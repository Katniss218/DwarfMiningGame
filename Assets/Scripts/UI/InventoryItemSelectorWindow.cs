using DwarfMiningGame.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityPlus.AssetManagement;

namespace DwarfMiningGame.UI
{
    public class InventoryItemSelectorWindow : MonoBehaviour
    {
        Inventory _inventory;
        Func<Inventory.ItemSlot, bool> _canEquip;
        Action<Inventory.ItemSlot> _onSelect;

        RectTransform _list;

        Dictionary<Inventory.ItemSlot, InventoryItemUI> _activeSlots = new Dictionary<Inventory.ItemSlot, InventoryItemUI>();

        void OnAfterSlotChanged( Inventory.ItemSlot slot )
        {
            // the selector might pass the same slot based on the conditions of the slot,
            // - so we need to check whether or not it's STILL available, and whether it's NOT available ANYMORE.

            if( _activeSlots.TryGetValue( slot, out InventoryItemUI iui ) )
            {
                if( !_canEquip( slot ) )
                {
                    RemoveItem( slot );
                }
                else
                {
                    iui.SetAmount( slot.Amount );
                }
            }
            else
            {
                if( _canEquip( slot ) )
                {
                    AddItem( slot );
                }
            }
        }

        void OnSlotAdded( Inventory.ItemSlot slot )
        {
            if( _canEquip( slot ) )
            {
                AddItem( slot );
            }
        }

        void OnSlotRemoved( Inventory.ItemSlot slot )
        {
            if( _activeSlots.ContainsKey( slot ) )
            {
                RemoveItem( slot );
            }
        }

        void AddItem( Inventory.ItemSlot slot )
        {
            InventoryItemUI iui = InventoryItemUI.Create( _list, slot );
            UIHelper.AddRaycastTarget( iui.gameObject );

            LeftClickAction c = iui.gameObject.AddComponent<LeftClickAction>();
            c.OnClick += () =>
            {
                _onSelect( slot );
                this.Close();
            };

            _activeSlots.Add( slot, iui );
        }

        void RemoveItem( Inventory.ItemSlot slot )
        {
            InventoryItemUI iui = _activeSlots[slot];

            Destroy( iui.gameObject );
            _activeSlots.Remove( slot );
        }

        public void Close()
        {
            Destroy( this.gameObject );
        }

        void OnDestroy()
        {
            if( _inventory != null )
            {
                _inventory.OnAfterSlotChanged -= this.OnAfterSlotChanged;
                _inventory.OnSlotAdded -= this.OnSlotAdded;
                _inventory.OnSlotRemoved -= this.OnSlotRemoved;
            }
        }

        /// <summary>
        /// Makes a window that equips an item to an equipment slot.
        /// </summary>
        public static InventoryItemSelectorWindow Create( Canvas parent, Inventory inventory, Func<Inventory.ItemSlot, bool> canEquipItem, Action<Inventory.ItemSlot> onSelect )
        {
            // a 1D vertical list of all the items that can be equipped in the selected slot.
            // 

            GameObject gameObject = UIHelper.UI( parent.transform, "Inventory Item Selector Window", new Vector2( 0.0f, 1.0f ), new Vector2( 455.0f, -85.0f ), new Vector2( 100.0f, 280.0f ) );

            InventoryItemSelectorWindow isw = gameObject.AddComponent<InventoryItemSelectorWindow>();
            isw._canEquip = canEquipItem;
            isw._onSelect = onSelect;
            isw._inventory = inventory;
            isw._inventory.OnAfterSlotChanged += isw.OnAfterSlotChanged;
            isw._inventory.OnSlotAdded += isw.OnSlotAdded;
            isw._inventory.OnSlotRemoved += isw.OnSlotRemoved;

            GameObject items = UIHelper.UIFill( gameObject.transform, "items", 0, 0, 0, 0 );

            GameObject content = UIHelper.MakeVerticalScrollRect( items );

            UIHelper.AddVerticalLayoutGroup( content, 5, 0, true );

            isw._list = (RectTransform)content.transform;

            foreach( var slot in inventory.GetItems() )
            {
                if( isw._canEquip( slot ) )
                {
                    isw.AddItem( slot );
                }
            }

            return isw;
        }
    }

    public class UIHelper
    {
        public static GameObject UIFill( Transform parent, string name, float left, float right, float top, float bottom )
        {
            Vector2 anchorMin = new Vector2( 0.0f, 0.0f );
            Vector2 anchorMax = new Vector2( 1.0f, 1.0f );
            Vector2 pivot = new Vector2( 0.5f, 0.5f );
            Vector2 anchoredPos = new Vector2( left, -top );
            Vector2 sizeDelta = new Vector2( -left - right, -top - bottom );

            return UI( parent, name, anchorMin, anchorMax, pivot, anchoredPos, sizeDelta );
        }

        /// <summary>
        /// Makes the UI element a raycast target.
        /// </summary>
        public static void AddRaycastTarget( GameObject go )
        {
            Image raycastImage = go.AddComponent<Image>();
            raycastImage.raycastTarget = true;
            raycastImage.color = new Color( 0, 0, 0, 0 ); // transparent.
        }

        public static GameObject UI( Transform parent, string name, Vector2 anchorPivot, Vector2 anchoredPos, Vector2 sizeDelta )
        {
            return UI( parent, name, anchorPivot, anchorPivot, anchorPivot, anchoredPos, sizeDelta );
        }

        public static GameObject UI( Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPos, Vector2 sizeDelta )
        {
            return UI( parent, name, anchorMin, anchorMax, new Vector2( (anchorMin.x + anchorMax.x) / 2, (anchorMin.y + anchorMax.y) / 2 ), anchoredPos, sizeDelta );
        }

        public static GameObject UI( Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 anchoredPos, Vector2 sizeDelta )
        {
            GameObject gameObject = new GameObject( name );
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.SetParent( parent.transform );
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.pivot = pivot;
            rectTransform.anchoredPosition = anchoredPos;
            rectTransform.sizeDelta = sizeDelta;

            return gameObject;
        }

        public static TMPro.TextMeshProUGUI AddText( GameObject obj, string text, TMPro.HorizontalAlignmentOptions horizontalAlign )
        {
            TMPro.TextMeshProUGUI tm = obj.AddComponent<TMPro.TextMeshProUGUI>();
            tm.fontSize = 16.0f;
            tm.raycastTarget = false;
            tm.horizontalAlignment = horizontalAlign;
            tm.verticalAlignment = TMPro.VerticalAlignmentOptions.Middle;
            tm.overflowMode = TMPro.TextOverflowModes.Overflow;
            tm.enableWordWrapping = false;
            tm.text = text;

            return tm;
        }

        public static void MakeForegroundImage( GameObject go )
        {
            Image image = go.AddComponent<Image>();
            image.raycastTarget = false;
            image.sprite = AssetRegistry<Sprite>.GetAsset( "Sprites/ui_beveled" );
            image.color = new Color( 0.5f, 0.5f, 0.5f, 1.0f );
            image.type = Image.Type.Sliced;
        }

        public static void MakeBackgroundImage( GameObject go )
        {
            Image image = go.AddComponent<Image>();
            image.raycastTarget = false;
            image.sprite = AssetRegistry<Sprite>.GetAsset( "Sprites/ui_beveled" );
            image.color = new Color( 0.25f, 0.25f, 0.25f, 1.0f );
            image.type = Image.Type.Sliced;
        }

        public static void AddVerticalLayoutGroup( GameObject go, int padding, int spacing, bool adjustToFit )
        {
            VerticalLayoutGroup vl = go.AddComponent<VerticalLayoutGroup>();
            vl.padding = new RectOffset( padding, padding, padding, padding );
            vl.spacing = spacing;
            vl.childControlWidth = true;
            vl.childControlHeight = false;
            vl.childScaleWidth = false;
            vl.childScaleHeight = false;
            vl.childForceExpandWidth = true;
            vl.childForceExpandHeight = false;

            if( adjustToFit )
            {
                ContentSizeFitter cs = go.AddComponent<ContentSizeFitter>();
                cs.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                cs.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
        }

        public static void AddColumnGridLayoutGroup( GameObject go, int padding, int spacing, Vector2 cellSize, GridLayoutGroup.Corner startCorner, TextAnchor childAlignment, int columnCount, bool adjustToFit )
        {
            GridLayoutGroup gl = go.AddComponent<GridLayoutGroup>();
            gl.padding = new RectOffset( padding, padding, padding, padding );
            gl.spacing = new Vector2( spacing, spacing );
            gl.cellSize = cellSize;
            gl.startCorner = startCorner;
            gl.startAxis = GridLayoutGroup.Axis.Horizontal;
            gl.childAlignment = childAlignment;
            gl.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gl.constraintCount = columnCount;

            if( adjustToFit )
            {
                ContentSizeFitter cs = go.AddComponent<ContentSizeFitter>();
                cs.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                cs.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
        }

        public static GameObject MakeVerticalScrollRect( GameObject obj )
        {
            GameObject items = UIHelper.UIFill( obj.transform, "items", 0, 0, 0, 0 );

            GameObject viewport = UIHelper.UIFill( items.transform, "viewport", 0, 0, 0, 0 );

            Image maskImage = viewport.AddComponent<Image>();
            Mask mask = viewport.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            GameObject content = UIHelper.UI( viewport.transform, "content", new Vector2( 0.0f, 1.0f ), new Vector2( 1, 1 ), Vector2.zero, new Vector2( 0.0f, 280.0f ) );

            MakeForegroundImage( items );

            ScrollRect scrollRect = items.AddComponent<ScrollRect>();
            scrollRect.content = (RectTransform)content.transform;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.inertia = true;
            scrollRect.decelerationRate = 0.5f;
            scrollRect.scrollSensitivity = 30f;
            scrollRect.viewport = (RectTransform)viewport.transform;

            return content;
        }
    }
}