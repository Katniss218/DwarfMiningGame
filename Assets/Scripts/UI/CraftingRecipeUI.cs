using DwarfMiningGame.Crafting;
using DwarfMiningGame.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DwarfMiningGame.UI
{
    public class CraftingRecipeUI : MonoBehaviour, IPointerClickHandler
    {
        CraftingRecipe _recipe;

        Action<CraftingRecipe> _onClick;

        bool _isLocked;
        bool _isEnabled;

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

        public void SetEnabled( bool isEnabled )
        {
            _isEnabled = isEnabled;
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
            _ingredientUIs.Clear();
            _resultUIs.Clear();
        }

        private Color GetTintColor()
        {
            if( _isLocked )
            {
                return new Color( 0, 0, 0, 1 );
            }
            return _isEnabled ? new Color( 1, 1, 1, 1 ) : new Color( 0.333f, 0.333f, 0.333f, 1 );
        }

        /// <summary>
        /// Creates the UI for a single crafting recipe entry.
        /// </summary>
        private void CreateEntries()
        {
            foreach( var ingredient in _recipe.Ingredients )
            {
                InventoryItemUI itemUI = InventoryItemUI.Create( _ingredientList, ingredient.Item, ingredient.Amount );
                itemUI.SetTint( GetTintColor() );
                this._ingredientUIs.Add( itemUI );
            }
            foreach( var result in _recipe.Results )
            {
                InventoryItemUI itemUI = InventoryItemUI.Create( _ingredientList, result.Item, result.Amount );
                itemUI.SetTint( GetTintColor() );
                this._ingredientUIs.Add( itemUI );
            }
        }

        public void OnPointerClick( PointerEventData eventData )
        {
            if( eventData.button == PointerEventData.InputButton.Left )
            {
                _onClick?.Invoke(_recipe);
            }
        }

        /// <summary>
        /// Creates the UI for a recipe.
        /// </summary>
        public static CraftingRecipeUI Create( RectTransform parent, CraftingRecipe recipe, bool isLocked, bool isEnabled, Action<CraftingRecipe> onClick )
        {
            // CraftingStationUI
            // - List of CraftingRecipeUI (fill width)
            // - - List of Ingredients
            // - - List of Results

            GameObject root = UIHelper.UI( parent, "container", new Vector2( 0, 0 ), new Vector2( 1, 0 ), new Vector2( parent.sizeDelta.x, InventoryItemUI.HEIGHT ) );

            UIHelper.AddScrollRect( root, false, true );

            GameObject ingredientList = UIHelper.UIFillPercent( parent, "ingredients", 0.0f, 0.5f, 0, 0 );
            UIHelper.MakeHorizontalLayoutGroup( ingredientList, 0, 5, false, false );
            GameObject resultList = UIHelper.UIFillPercent( parent, "ingredients", 0.5f, 0.0f, 0, 0 );
            UIHelper.MakeHorizontalLayoutGroup( resultList, 0, 5, false, true );

            UIHelper.MakeRaycastTarget( root );
            CraftingRecipeUI ui = root.AddComponent<CraftingRecipeUI>();
            ui._onClick = onClick;
            ui._ingredientList = ingredientList.GetComponent<RectTransform>();
            ui._resultList = resultList.GetComponent<RectTransform>();
            ui.SetRecipe( recipe, isLocked );
            ui.SetEnabled( isEnabled );

            return ui;
        }
    }
}