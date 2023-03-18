using DwarfMiningGame.Inventories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityPlus.AssetManagement;

namespace DwarfMiningGame.UI
{
    public sealed class PlayerInventoryUI : MonoBehaviour
    {
        PlayerInventory _inventory;
        public PlayerInventory Inventory
        {
            get => _inventory;
            set
            {
                if( this._inventory != null )
                {
                    this._inventory.OnAfterMoneyChanged -= this.OnMoneyChanged;
                    this._inventory.OnAfterSlotChanged -= this.OnAfterSlotChanged;
                    this._inventory.OnSlotAdded -= this.OnSlotAdded;
                    this._inventory.OnSlotRemoved -= this.OnSlotRemoved;
                    this._inventory.OnAfterEquipmentChanged -= this.OnAfterEquipmentChanged;
                }

                this._inventory = value;

                if( this._inventory != null )
                {
                    this._inventory.OnAfterMoneyChanged += this.OnMoneyChanged;
                    this._inventory.OnAfterSlotChanged += this.OnAfterSlotChanged;
                    this._inventory.OnSlotAdded += this.OnSlotAdded;
                    this._inventory.OnSlotRemoved += this.OnSlotRemoved;
                    this._inventory.OnAfterEquipmentChanged += this.OnAfterEquipmentChanged;
                }
            }
        }

        [SerializeField] TMPro.TextMeshProUGUI _moneyText;

        [SerializeField] RectTransform _equipmentList;
        [SerializeField] RectTransform _itemsList;

        List<InventoryEquipmentUI> _equipmentUIs = new List<InventoryEquipmentUI>();

        Dictionary<Inventory.ItemSlot, ItemUI> _itemUIs = new Dictionary<Inventory.ItemSlot, ItemUI>();

        void OnAfterEquipmentChanged( int index, Inventory.ItemSlot slot )
        {
            _equipmentUIs[index].SetSlot( slot );
        }

        void OnAfterSlotChanged( Inventory.ItemSlot slot )
        {
            if( _itemUIs.TryGetValue( slot, out ItemUI ui ) )
            {
                ui.SetAmount( slot.Amount );
            }
            else
            {
                Debug.LogError( $"Inventory desync, slot {slot} wasn't added to the inventory UI." );
            }
        }

        void OnSlotAdded( Inventory.ItemSlot slot )
        {
            if( !_itemUIs.ContainsKey( slot ) )
            {
                CreateItemUI( slot );
            }
        }

        void OnSlotRemoved( Inventory.ItemSlot slot )
        {
            if( _itemUIs.TryGetValue( slot, out ItemUI ui ) )
            {
                Destroy( ui.gameObject );
                _itemUIs.Remove( slot );
            }
        }

        void CreateItemUI( Inventory.ItemSlot slot )
        {
            ItemUI ui = ItemUI.Create( _itemsList, slot.Item, slot.Amount );
            _itemUIs.Add( slot, ui );
        }

        private void SetMoneyText( float currentMoney )
        {
            _moneyText.text = FormatMoney(currentMoney);
        }

        void OnMoneyChanged( float newMoney )
        {
            SetMoneyText( newMoney );
        }

        InventorySelectorContextMenu currentEquipSelectorWindow = null;

        /// <summary>
        /// Creates a new equipment slot UI and binds it to the specified slot of the inventory.
        /// </summary>
        /// <param name="canEquipItem">Function that checks whether or not the item is displayed in the selection window for this slot.</param>
        /// <param name="onSelect">The action to perform upon selecting the item for this slot.</param>
        public void CreateEquipmentSlotUI( Func<Inventory.ItemSlot, bool> canEquipItem, Action<Inventory.ItemSlot> onSelect )
        {
            InventoryEquipmentUI ui = InventoryEquipmentUI.Create( _equipmentList, () =>
            {
                if( currentEquipSelectorWindow != null )
                {
                    currentEquipSelectorWindow.Close();
                }
                currentEquipSelectorWindow = InventorySelectorContextMenu.Create( GameManager.ContextMenuCanvas, this.Inventory, true, canEquipItem, onSelect );
            } );

            _equipmentUIs.Add( ui );
        }

        public static string FormatMoney( float value )
        {
            return $"¤{value.ToString( "#.00", CultureInfo.InvariantCulture )}";
        }


        public static PlayerInventoryUI Create( RectTransform panel, PlayerInventory inventory )
        {
            GameObject root = UIHelper.UI( panel, "Inventory UI", new Vector2( 0, 1 ), Vector2.zero, new Vector2( 455, 365 ) );

            UIHelper.MakeBackground( root );

            GameObject moneyGO = UIHelper.UI( root.transform, "$money", new Vector2( 1, 1 ), new Vector2( -5, -5 ), new Vector2( 200, 25 ) );
            TMPro.TextMeshProUGUI moneyText = UIHelper.MakeText( moneyGO, "<$money>", TMPro.HorizontalAlignmentOptions.Right );

            GameObject items = UIHelper.UI( root.transform, "items", new Vector2( 0, 0 ), new Vector2( 0, 1 ), new Vector2( 0, -42.5f ), new Vector2( 385, -85 ) );
            UIHelper.MakeForeground( items );

            GameObject itemsContent = UIHelper.AddScrollRect( items, false, true );
            UIHelper.MakeColumnGridLayoutGroup( itemsContent, 5, 0, new Vector2( ItemUI.WIDTH, ItemUI.HEIGHT ), GridLayoutGroup.Corner.UpperLeft, TextAnchor.UpperLeft, 5, true );

            GameObject equipment = UIHelper.UI( root.transform, "equipment", new Vector2( 1, 0 ), new Vector2( 1, 1 ), new Vector2( 0, -42.5f ), new Vector2( 70, -85 ) );

            GameObject equipmentContent = UIHelper.AddScrollRect( equipment, false, true );
            UIHelper.MakeVerticalLayoutGroup( equipmentContent, 0, 0, true );


            PlayerInventoryUI ui = root.AddComponent<PlayerInventoryUI>();
            ui.Inventory = inventory;
            ui._itemsList = (RectTransform)itemsContent.transform;
            ui._moneyText = moneyText;
            ui._equipmentList = (RectTransform)equipmentContent.transform;

            foreach( var item in inventory.GetItems() )
            {
                ui.CreateItemUI( item );
            }

            ui.CreateEquipmentSlotUI( ( s ) => s.Item is ItemPickaxe, ( s ) => ui.Inventory.MainHand = s );
            ui.CreateEquipmentSlotUI( ( s ) => s.Item is Item, ( s ) => ui.Inventory.OffHand = s );
            ui.CreateEquipmentSlotUI( ( s ) => s.Item is ItemBag, ( s ) => ui.Inventory.Backpack = s );

            ui.OnMoneyChanged( inventory.Money );

            return ui;
        }
    }
}