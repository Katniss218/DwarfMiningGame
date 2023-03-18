using DwarfMiningGame.Crafting;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.UI
{
    /// <summary>
    /// The user interface for a <see cref="CraftingStationBehaviour"/>.
    /// </summary>
    public class CraftingStationUI : MonoBehaviour
    {
        // #############################

        // Design / Assumptions:
        // - A scrollable vertical list of recipes.
        //   - Each recipe displays ingredients and results.
        //   - Locked recipes are black silhouettes of items with a lock symbol.
        //   - Recipes with ingredients missing from the inventory are greyed out.

        // - Click on a recipe to craft it.

        // #############################

        public CraftingStationBehaviour CraftingStation { get; private set; }

        // holds the action that happens when the recipe entry is clicked. It's a way to pass the inventory through.
        Action<CraftingRecipe> _onClickEntry;

        RectTransform _list;
        Dictionary<CraftingRecipe, CraftingRecipeUI> _entryUIs = new Dictionary<CraftingRecipe, CraftingRecipeUI>();

        PlayerData _player; // quite ugly.

#warning TODO - Look here, I think the problems with the cached variables in CraftingStationBehaviour can be solved by including the behaviour here. Since UI should know about what behaviour it's UI-ing.
        /// <summary>
        /// (Re)Binds the UI to a specific crafting station.
        /// </summary>
        public void SetCraftingStation( CraftingStationBehaviour craftingStation )
        {
            CraftingStation = craftingStation;
            Redraw();
        }

        public void SetLockedRecipe( CraftingRecipe recipe, bool isLocked )
        {
            if( _entryUIs.TryGetValue( recipe, out CraftingRecipeUI ui ) )
            {
                ui.SetLocked( isLocked ); // Gonna redraw itself.
            }
        }


        public void SetEnabledRecipe( CraftingRecipe recipe, bool isEnabled )
        {
            if( _entryUIs.TryGetValue( recipe, out CraftingRecipeUI ui ) )
            {
                ui.SetEnabled( isEnabled ); // Gonna redraw itself.
            }
        }


        public void OnRecipeLockChanged( CraftingRecipe recipe, bool isLocked )
        {
            // listener

            if( _entryUIs.TryGetValue( recipe, out CraftingRecipeUI ui ) )
            {
                ui.SetLocked( isLocked );
            }
        }

        private void Redraw()
        {
            DeleteRecipes();
            AddRecipes();
        }

        private void DeleteRecipes()
        {
            foreach( var ui in _entryUIs.Values )
            {
                Destroy( ui.gameObject );
            }
            _entryUIs.Clear();
        }

        private void AddRecipes()
        {
            foreach( var recipe in CraftingStation.CraftingStation.Recipes )
            {
                CraftingRecipeUI ui = CraftingRecipeUI.Create( _list, recipe, false, true, _onClickEntry );
                _entryUIs.Add( recipe, ui );
            }
        }

        public static CraftingStationUI Create( RectTransform mainCanvas, CraftingStationBehaviour craftingStation, Action<CraftingRecipe> onClickRecipe )
        {
            GameObject rootGO = UIHelper.UI( mainCanvas.transform, "crafting station", new Vector2( 0.5f, 0.5f ), Vector2.zero, new Vector2( 300, 280 ) );

            GameObject content = UIHelper.AddScrollRect( rootGO, false, true );
            UIHelper.MakeBackground( rootGO );
            UIHelper.MakeVerticalLayoutGroup( content, 5, 5, true );

            CraftingStationUI ui = rootGO.AddComponent<CraftingStationUI>();
            ui._list = content.GetComponent<RectTransform>();
            ui._onClickEntry = onClickRecipe;
            ui._player = GameManager.Instance.PlayerData;
            ui.SetCraftingStation( craftingStation );

            return ui;
        }
    }
}