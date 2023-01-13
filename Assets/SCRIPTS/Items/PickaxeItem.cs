using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Items
{ 
    [CreateAssetMenu( fileName = "pickaxe item", menuName = "DwarfMiningGame/Item-Pickaxe", order = 310 )]
    public class PickaxeItem : Item
    {
        [field: SerializeField]
        public float MiningPower { get; set; }
    }
}