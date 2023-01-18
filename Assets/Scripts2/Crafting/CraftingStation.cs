using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.Crafting
{
    /// <summary>
    /// A definition of a crafting station
    /// </summary>
    [CreateAssetMenu( fileName = "crafting_station", menuName = "DwarfMiningGame/Crafting Station", order = 500 )]
    public class CraftingStation : ScriptableObject, IIdentifiable
    {
        [field: SerializeField]
        public string ID { get; set; }

        [field: SerializeField]
        public CraftingRecipe[] AvailableRecipes { get; set; }
    }
}