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


        CraftingStation _craftingStation;

        Dictionary<CraftingRecipe, CraftingRecipeUI> _uis = new Dictionary<CraftingRecipe, CraftingRecipeUI>();

        PlayerData _player; // quite ugly.

        void AddRecipe( CraftingRecipe recipe )
        {
            // add recipe, draw.
        }

        void RemoveRecipe( CraftingRecipe recipe )
        {

        }

        public void SetCraftingStation( CraftingStation craftingStation )
        {
            _craftingStation = craftingStation;
            Redraw();
        }

        void Redraw()
        {
            // delete previous recipes (if any), create new ones.
        }

        public static CraftingStationUI Create( Canvas mainCanvas, CraftingStation craftingStation )
        {
            GameObject rootGO = UIHelper.UI( mainCanvas.transform, "crafting station", new Vector2( 0.5f, 0.5f ), Vector2.zero, new Vector2( 100, 280 ) );

            UIHelper.AddScrollRect( rootGO, false, true );
            UIHelper.MakeBackground( rootGO );

            CraftingStationUI cs = rootGO.AddComponent<CraftingStationUI>();
            cs.SetCraftingStation( craftingStation );

            return cs;
        }
    }
}