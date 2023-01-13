using DwarfMiningGame.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.Crafting
{
    [CreateAssetMenu( fileName = "crafting_recipe", menuName = "DwarfMiningGame/Crafting Recipe", order = 500 )]
    public class CraftingRecipe : ScriptableObject
    {
        [Serializable]
        public class RecipeItem
        {
            [field: SerializeField]
            public Item Item { get; set; }

            [field: SerializeField]
            public int Amount { get; set; }
        }

        [field: SerializeField]
        public RecipeItem[] Ingredients { get; set; }

        [field: SerializeField]
        public RecipeItem Result { get; set; }
    }
}
