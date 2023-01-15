using DwarfMiningGame.Items;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.UI
{
    public class PlayerInventoryUI : InventoryUI
    {
        public PlayerInventory PlayerInventory { get => (PlayerInventory)Inventory; set => Inventory = value; }

        [SerializeField] TMPro.TextMeshProUGUI _moneyText;

        protected override void Awake()
        {
            PlayerInventory.OnAfterMoneyChanged += OnMoneyChanged;
            base.Awake();
        }

        private void SetMoneyText( float currentMoney )
        {
            // money formatted with 2 decimal points, always.
            _moneyText.text = $"¤{currentMoney.ToString( "#.00", CultureInfo.InvariantCulture )}";
        }

        void OnMoneyChanged( float newMoney )
        {
            SetMoneyText( newMoney );
        }
    }
}