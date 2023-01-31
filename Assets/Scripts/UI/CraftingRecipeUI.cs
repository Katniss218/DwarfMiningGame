using DwarfMiningGame.Crafting;
using DwarfMiningGame.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.UI
{
    public class CraftingRecipeUI : MonoBehaviour
    {
        CraftingRecipe _recipe;
        Inventory _targetInventory; // inventory of whatever entity/player is doing the crafting.


        bool _isLocked;

        RectTransform _ingredientList;
        RectTransform _resultList;

        List<InventoryItemUI> _ingredientUIs = new List<InventoryItemUI>();
        List<InventoryItemUI> _resultUIs = new List<InventoryItemUI>();


        // sets the recipe displayed by this UI.
        public void SetRecipe( CraftingRecipe recipe, bool isLocked )
        {
            _recipe = recipe;
            _isLocked = isLocked;
            Redraw();
        }

        public void SetLocked( bool isLocked )
        {
            _isLocked = isLocked;
            Redraw();
        }

        void Redraw()
        {
            DeleteEntries();
            CreateEntries();
        }

        private void DeleteEntries()
        {
            foreach( var entry in _ingredientUIs.Union( _resultUIs ) )
            {
                Destroy( entry.gameObject );
            }
        }

        /// <summary>
        /// Creates the UI for a single crafting recipe entry.
        /// </summary>
        private void CreateEntries()
        {
            foreach( var ingredient in _recipe.Ingredients )
            {
                InventoryItemUI itemUI = InventoryItemUI.Create( _ingredientList, ingredient.Item, ingredient.Amount );
                this._ingredientUIs.Add( itemUI );
            }
            foreach( var result in _recipe.Results )
            {
                InventoryItemUI itemUI = InventoryItemUI.Create( _ingredientList, result.Item, result.Amount );
                this._ingredientUIs.Add( itemUI );
            }
        }

        /// <summary>
        /// Creates the UI for a recipe.
        /// </summary>
        public static CraftingRecipeUI Create( RectTransform parent, CraftingRecipe recipe, bool isLocked )
        {
            // CraftingStationUI
            // - List of CraftingRecipeUI (fill width)
            // - - List of Ingredients
            // - - List of Results

            GameObject container = UIHelper.UI( parent, "container", new Vector2( 0, 0 ), new Vector2( 1, 0 ), new Vector2( parent.sizeDelta.x, InventoryItemUI.HEIGHT ) );

            UIHelper.AddScrollRect( container, false, true );

            GameObject ingredientList = UIHelper.UIFillPercent( parent, "ingredients", 0.0f, 0.5f, 0, 0 );
            UIHelper.MakeHorizontalLayoutGroup( ingredientList, 0, 5, false, false );
            GameObject resultList = UIHelper.UIFillPercent( parent, "ingredients", 0.5f, 0.0f, 0, 0 );
            UIHelper.MakeHorizontalLayoutGroup( resultList, 0, 5, false, true );

            CraftingRecipeUI ui = container.AddComponent<CraftingRecipeUI>();
            ui._ingredientList = ingredientList.GetComponent<RectTransform>();
            ui._resultList = resultList.GetComponent<RectTransform>();
            ui.SetRecipe( recipe, isLocked );

            return ui;
        }
    }
}