using DwarfMiningGame.Crafting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.UI
{
    public class CraftingStationUI : MonoBehaviour
    {
        // a scrollable vertical list of all recipes.
        // Click on recipe to craft.
        // locked recipes are black shadows of items with a lock symbol.
        // recipes with not enough items are greyed out.

        public CraftingStation CraftingStation { get; private set; }

        Action<CraftingRecipe> _onClickRecipe;

        RectTransform _list;
        Dictionary<CraftingRecipe, CraftingRecipeUI> _uis = new Dictionary<CraftingRecipe, CraftingRecipeUI>();

        PlayerData _player; // quite ugly.

        public void SetCraftingStation( CraftingStation craftingStation )
        {
            CraftingStation = craftingStation;
            Redraw();
        }

        public void OnRecipeLockChanged( CraftingRecipe recipe, bool isLocked )
        {
            // listener

            if( _uis.TryGetValue( recipe, out CraftingRecipeUI ui ) )
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
            foreach( var ui in _uis.Values )
            {
                Destroy( ui.gameObject );
            }
            _uis.Clear();
        }

        private void AddRecipes()
        {
            foreach( var recipe in CraftingStation.Recipes )
            {
                CraftingRecipeUI ui = CraftingRecipeUI.Create( _list, recipe, false, true, _onClickRecipe );
                _uis.Add( recipe, ui );
            }
        }

        public static CraftingStationUI Create( RectTransform mainCanvas, CraftingStation craftingStation, Action<CraftingRecipe> onClickRecipe )
        {
            GameObject rootGO = UIHelper.UI( mainCanvas.transform, "crafting station", new Vector2( 0.5f, 0.5f ), Vector2.zero, new Vector2( 300, 280 ) );

            GameObject content = UIHelper.AddScrollRect( rootGO, false, true );
            UIHelper.MakeBackground( rootGO );
            UIHelper.MakeVerticalLayoutGroup( content, 5, 5, true );

            CraftingStationUI ui = rootGO.AddComponent<CraftingStationUI>();
            ui._list = content.GetComponent<RectTransform>();
            ui._onClickRecipe = onClickRecipe;
            ui._player = GameManager.Instance.PlayerData;
            ui.SetCraftingStation( craftingStation );

            return ui;
        }
    }
}