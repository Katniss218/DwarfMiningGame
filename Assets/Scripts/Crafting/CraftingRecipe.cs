using DwarfMiningGame.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.Crafting
{
    /// <summary>
    /// A definition of a crafting recipe.
    /// </summary>
    [CreateAssetMenu( fileName = "crafting_recipe", menuName = "DwarfMiningGame/Crafting Recipe", order = 510 )]
    public class CraftingRecipe : ScriptableObject, IIdentifiable
    {
        [Serializable]
        public class Entry
        {
            [field: SerializeField]
            public Item Item { get; set; }

            [field: SerializeField]
            public int Amount { get; set; } = 1;
        }

        [field: SerializeField]
        public string ID { get; set; } = "recipe.invalid";

        [field: SerializeField]
        public Entry[] Ingredients { get; set; }

        [field: SerializeField]
        public Entry[] Results { get; set; }
    }

    public static class InventoryEx_Recipe
    {
        public static bool HasEnoughItems( this Inventory inv, CraftingRecipe recipe, int count = 1 )
        {
            foreach( var entry in recipe.Ingredients )
            {
                if( inv.GetAmount( entry.Item ) < entry.Amount * count )
                {
                    return false;
                }
            }

            return true;
        }

        public static bool HasEnoughSpaceLeft( this Inventory inv, CraftingRecipe recipe, int count = 1 )
        {
            float left = inv.GetSpaceLeft();

            foreach( var entry in recipe.Ingredients ) // Ingredients should be removed before the result is added, effectively freeing up their slots.
            {
                left += entry.Amount * count * entry.Item.Size;
            }

            foreach( var entry in recipe.Results )
            {
                left -= entry.Amount * count * entry.Item.Size;
            }

            return left >= 0;
        }

        public static void TryCraft( this Inventory inv, CraftingRecipe recipe, int count = 1 )
        {
            if( !inv.HasEnoughItems( recipe, count ) )
            {
                Debug.LogWarning( $"Not enough ingredients in inventory {inv} to craft {recipe}." );
                return;
            }
            if( !inv.HasEnoughSpaceLeft( recipe, count ) )
            {
                Debug.LogWarning( $"Not enough space in inventory {inv} to craft {recipe}." );
                return;
            }

            foreach( var ing in recipe.Ingredients ) // Ingredients should be removed before the result is added, effectively freeing up their slots.
            {
                inv.TryRemove( ing.Item, ing.Amount * count );
            }

            foreach( var ing in recipe.Results )
            {
                inv.TryAdd( ing.Item, ing.Amount * count );
            }
        }
    }
}