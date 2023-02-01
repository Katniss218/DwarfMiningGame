using DwarfMiningGame.Crafting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame
{
    [Serializable]
    public class PlayerData
    {
        // PlayerData stores additional information relating to a player.
        // PlayerData needs to be saved.

        [Serializable]
        public class CraftingRecipeData
        {
            [field: SerializeField]
            public bool IsUnlocked { get; set; } = false;
        }

        public Dictionary<CraftingRecipe, CraftingRecipeData> CraftingRecipes;
    }
}