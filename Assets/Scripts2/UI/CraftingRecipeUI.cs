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


        List<InventoryItemUI> _ingredientUIs = new List<InventoryItemUI>();
        List<InventoryItemUI> _resultUIs = new List<InventoryItemUI>();


        void Redraw()
        {
            // delete the previous ingredients, results (amount might change).
            // create new ones.
        }


        // sets the recipe displayed by this UI.
        public void SetRecipe( CraftingRecipe recipe, bool isLocked )
        {
            _recipe = recipe;
            _isLocked = isLocked;
            Redraw();
        }


        public void SetLocked( bool isLocked )
        {
            Redraw();
        }
    }
}