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
    /// A class that lets you select an item from the inventory.
    /// </summary>
    public class InventorySelectorContextMenu : ContextMenu
    {
        Inventory _inventory;
        Func<Inventory.ItemSlot, bool> _canEquip;
        Action<Inventory.ItemSlot> _onSelect;

        bool _closeOnSelect = false;

        RectTransform _list;

        Dictionary<Inventory.ItemSlot, ItemUI> _activeSlots = new Dictionary<Inventory.ItemSlot, ItemUI>();

        void OnAfterSlotChanged( Inventory.ItemSlot slot )
        {
            // the selector might pass the same slot based on the conditions of the slot,
            // - so we need to check whether or not it's STILL available, and whether it's NOT available ANYMORE.

            if( _activeSlots.TryGetValue( slot, out ItemUI iui ) )
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
            ItemUI iui = ItemUI.Create( _list, slot.Item, slot.Amount );
            UIHelper.MakeRaycastTarget( iui.gameObject );

            LeftClickAction c = iui.gameObject.AddComponent<LeftClickAction>();
            c.OnClick += () =>
            {
                _onSelect( slot );
                if( _closeOnSelect )
                {
                    this.Close();
                }
            };

            _activeSlots.Add( slot, iui );
        }

        void RemoveItem( Inventory.ItemSlot slot )
        {
            ItemUI iui = _activeSlots[slot];

            Destroy( iui.gameObject );
            _activeSlots.Remove( slot );
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
        public static InventorySelectorContextMenu Create( RectTransform parent, Inventory inventory, bool closeOnSelect, Func<Inventory.ItemSlot, bool> canEquipItem, Action<Inventory.ItemSlot> onSelect )
        {
            // a 1D vertical list of all the items that can be equipped in the selected slot.
            // 

            GameObject gameObject = UIHelper.UI( parent, "Inventory Item Selector Window", new Vector2( 0.0f, 1.0f ), new Vector2( 455.0f, -85.0f ), new Vector2( 100.0f, 280.0f ) );

            InventorySelectorContextMenu isw = gameObject.AddComponent<InventorySelectorContextMenu>();
            isw._canEquip = canEquipItem;
            isw._onSelect = onSelect;
            isw._closeOnSelect = closeOnSelect;
            isw._inventory = inventory;
            isw._inventory.OnAfterSlotChanged += isw.OnAfterSlotChanged;
            isw._inventory.OnSlotAdded += isw.OnSlotAdded;
            isw._inventory.OnSlotRemoved += isw.OnSlotRemoved;

            GameObject items = UIHelper.UIFill( gameObject.transform, "items", 0, 0, 0, 0 );
            UIHelper.MakeForeground( items );

            GameObject content = UIHelper.AddScrollRect( items, false, true );

            UIHelper.MakeVerticalLayoutGroup( content, 5, 0, true );

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

    public static class UIHelper
    {
#warning TODO - move this to a separate file and clean up a bit, possibly with style parameters that are static fields.

        // Make - doesn't create any new objects.
        // Add - creates new objects.

        public static GameObject UIFill( Transform parent, string name, float left, float right, float top, float bottom )
        {
            Vector2 anchorMin = new Vector2( 0.0f, 0.0f );
            Vector2 anchorMax = new Vector2( 1.0f, 1.0f );
            Vector2 pivot = new Vector2( 0.5f, 0.5f );
            Vector2 anchoredPos = new Vector2( left, -top );
            Vector2 sizeDelta = new Vector2( -left - right, -top - bottom );

            return UI( parent, name, anchorMin, anchorMax, pivot, anchoredPos, sizeDelta );
        }

        public static GameObject UIFillPercent( Transform parent, string name, float left, float right, float top, float bottom )
        {
            Vector2 anchorMin = new Vector2( left, bottom );
            Vector2 anchorMax = new Vector2( 1.0f - right, 1.0f - top );
            Vector2 pivot = new Vector2( 0.5f, 0.5f );
            Vector2 anchoredPos = new Vector2( left, -top );
            Vector2 sizeDelta = new Vector2( -left - right, -top - bottom );

            return UI( parent, name, anchorMin, anchorMax, pivot, anchoredPos, sizeDelta );
        }

        /// <summary>
        /// Makes the UI element a raycast target.
        /// </summary>
        public static void MakeRaycastTarget( GameObject go )
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

        public static TMPro.TextMeshProUGUI MakeText( GameObject obj, string text, TMPro.HorizontalAlignmentOptions horizontalAlign )
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

        public static readonly Color FOREGROUND_COLOR = new Color( 0.5f, 0.5f, 0.5f, 1.0f );
        public static readonly Color BACKGROUND_COLOR = new Color( 0.25f, 0.25f, 0.25f, 1.0f );

        public static void MakeForeground( GameObject go )
        {
            Image image = go.AddComponent<Image>();
            image.raycastTarget = false;
            image.sprite = AssetRegistry<Sprite>.GetAsset( "Sprites/ui_beveled" );
            image.color = FOREGROUND_COLOR;
            image.type = Image.Type.Sliced;
        }

        public static void MakeBackground( GameObject go )
        {
            Image image = go.AddComponent<Image>();
            image.raycastTarget = false;
            image.sprite = AssetRegistry<Sprite>.GetAsset( "Sprites/ui_beveled" );
            image.color = BACKGROUND_COLOR;
            image.type = Image.Type.Sliced;
        }

        public static void MakeVerticalLayoutGroup( GameObject go, int padding, int spacing, bool containerFitsContents, bool reversed = false )
        {
            VerticalLayoutGroup vl = go.AddComponent<VerticalLayoutGroup>();
            vl.childAlignment = reversed ? TextAnchor.LowerRight : TextAnchor.UpperLeft;
            vl.padding = new RectOffset( padding, padding, padding, padding );
            vl.spacing = spacing;
            vl.childControlWidth = true;
            vl.childControlHeight = false;
            vl.childScaleWidth = false;
            vl.childScaleHeight = false;
            vl.childForceExpandWidth = true;
            vl.childForceExpandHeight = false;

            if( containerFitsContents )
            {
                ContentSizeFitter cs = go.AddComponent<ContentSizeFitter>();
                cs.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                cs.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
        }

        public static void MakeHorizontalLayoutGroup( GameObject go, int padding, int spacing, bool containerFitsContents, bool reversed = false )
        {
            HorizontalLayoutGroup vl = go.AddComponent<HorizontalLayoutGroup>();
            vl.childAlignment = reversed ? TextAnchor.LowerRight : TextAnchor.UpperLeft;
            vl.padding = new RectOffset( padding, padding, padding, padding );
            vl.spacing = spacing;
            vl.childControlWidth = false;
            vl.childControlHeight = true;
            vl.childScaleWidth = false;
            vl.childScaleHeight = false;
            vl.childForceExpandWidth = false;
            vl.childForceExpandHeight = true;

            if( containerFitsContents )
            {
                ContentSizeFitter cs = go.AddComponent<ContentSizeFitter>();
                cs.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                cs.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            }
        }

        public static void MakeColumnGridLayoutGroup( GameObject go, int padding, int spacing, Vector2 cellSize, GridLayoutGroup.Corner startCorner, TextAnchor childAlignment, int columnCount, bool containerFitsContents )
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

            if( containerFitsContents )
            {
                ContentSizeFitter cs = go.AddComponent<ContentSizeFitter>();
                cs.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                cs.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
        }

        /// <summary>
        /// Doesn't include a scrollbar, doesn't include any layout for the content.
        /// </summary>
        /// <returns>The gameobject that will contain the contents.</returns>
        public static GameObject AddScrollRect( GameObject obj, bool horizontal, bool vertical )
        {
            GameObject items = UIHelper.UIFill( obj.transform, "items", 0, 0, 0, 0 );

            GameObject viewport = UIHelper.UIFill( items.transform, "viewport", 0, 0, 0, 0 );

            Image maskImage = viewport.AddComponent<Image>();
            Mask mask = viewport.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            GameObject content = UIHelper.UI( viewport.transform, "content", new Vector2( 0.0f, 1.0f ), new Vector2( 1, 1 ), Vector2.zero, new Vector2( 0.0f, 280.0f ) );

            ScrollRect scrollRect = items.AddComponent<ScrollRect>();
            scrollRect.content = (RectTransform)content.transform;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.horizontal = horizontal;
            scrollRect.vertical = vertical;
            scrollRect.inertia = true;
            scrollRect.decelerationRate = 0.5f;
            scrollRect.scrollSensitivity = 30f;
            scrollRect.viewport = (RectTransform)viewport.transform;

            return content;
        }
    }
}