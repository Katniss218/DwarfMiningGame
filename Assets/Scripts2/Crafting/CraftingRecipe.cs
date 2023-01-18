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
            public int Amount { get; set; }
        }

        [field: SerializeField]
        public string ID { get; set; }

        [field: SerializeField]
        public Entry[] Ingredients { get; set; }

        [field: SerializeField]
        public Entry[] Results { get; set; }
    }
}