using DwarfMiningGame.Items;
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

        Dictionary<Inventory.ItemSlot, InventoryItemUI> _itemUIs = new Dictionary<Inventory.ItemSlot, InventoryItemUI>();

        void OnAfterEquipmentChanged( int index, Inventory.ItemSlot slot )
        {
            _equipmentUIs[index].SetSlot( slot );
        }

        void OnAfterSlotChanged( Inventory.ItemSlot slot )
        {
            if( _itemUIs.TryGetValue( slot, out InventoryItemUI ui ) )
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
            if( _itemUIs.TryGetValue( slot, out InventoryItemUI ui ) )
            {
                Destroy( ui.gameObject );
                _itemUIs.Remove( slot );
            }
        }

        void CreateItemUI( Inventory.ItemSlot slot )
        {
            InventoryItemUI ui = InventoryItemUI.Create( _itemsList, slot );
            _itemUIs.Add( slot, ui );
        }

        private void SetMoneyText( float currentMoney )
        {
            _moneyText.text = $"¤{currentMoney.ToString( "#.00", CultureInfo.InvariantCulture )}";
        }

        void OnMoneyChanged( float newMoney )
        {
            SetMoneyText( newMoney );
        }

        InventoryItemSelectorWindow currentEquipSelectorWindow = null;

        public void RegisterEquipment( Func<Inventory.ItemSlot, bool> canEquipItem, Action<Inventory.ItemSlot> onSelect )
        {
            InventoryEquipmentUI ui = InventoryEquipmentUI.Create( _equipmentList, () =>
            {
                if( currentEquipSelectorWindow != null )
                {
                    currentEquipSelectorWindow.Close();
                }
                currentEquipSelectorWindow = InventoryItemSelectorWindow.Create( GameObject.Find( "Context Menu Canvas" ).GetComponent<Canvas>(), this.Inventory, canEquipItem, onSelect );
            } );

            _equipmentUIs.Add( ui );
        }


        public static PlayerInventoryUI Create( RectTransform panel, PlayerInventory inventory )
        {
            GameObject root = UIHelper.UI( panel, "Inventory UI", new Vector2( 0, 1 ), Vector2.zero, new Vector2( 455, 365 ) );

            UIHelper.MakeBackgroundImage( root );

            GameObject moneyGO = UIHelper.UI( root.transform, "$money", new Vector2( 1, 1 ), new Vector2( -5, -5 ), new Vector2( 200, 25 ) );
            TMPro.TextMeshProUGUI moneyText = UIHelper.AddText( moneyGO, "<$money>", TMPro.HorizontalAlignmentOptions.Right );

            GameObject items = UIHelper.UI( root.transform, "items", new Vector2( 0, 0 ), new Vector2( 0, 1 ), new Vector2( 0, -42.5f ), new Vector2( 385, -85 ) );
            UIHelper.MakeForegroundImage( items );

            GameObject itemsContent = UIHelper.MakeVerticalScrollRect( items );
            UIHelper.AddColumnGridLayoutGroup( itemsContent, 5, 0, new Vector2( 75, 36 ), GridLayoutGroup.Corner.UpperLeft, TextAnchor.UpperLeft, 5, true );

            GameObject equipment = UIHelper.UI( root.transform, "equipment", new Vector2( 1, 0 ), new Vector2( 1, 1 ), new Vector2( 0, -42.5f ), new Vector2( 70, -85 ) );

            GameObject equipmentContent = UIHelper.MakeVerticalScrollRect( equipment );
            UIHelper.AddVerticalLayoutGroup( equipmentContent, 0, 0, true );


            PlayerInventoryUI ui = root.AddComponent<PlayerInventoryUI>();
            ui.Inventory = inventory;
            ui._itemsList = (RectTransform)itemsContent.transform;
            ui._moneyText = moneyText;
            ui._equipmentList = (RectTransform)equipmentContent.transform;

            foreach( var item in inventory.GetItems() )
            {
                ui.CreateItemUI( item );
            }

            ui.RegisterEquipment( ( s ) => s.Item is ItemPickaxe, ( s ) => ui.Inventory.MainHand = s );
            ui.RegisterEquipment( ( s ) => s.Item is Item, ( s ) => ui.Inventory.OffHand = s );
            ui.RegisterEquipment( ( s ) => s.Item is ItemBag, ( s ) => ui.Inventory.Bag = s );

            ui.OnMoneyChanged( inventory.Money );

            return ui;
        }
    }
}