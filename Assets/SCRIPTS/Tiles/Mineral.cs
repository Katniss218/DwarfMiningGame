using DwarfMiningGame.Loot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.Tiles
{
    [CreateAssetMenu( fileName = "mineral", menuName = "DwarfMiningGame/Mineral", order = 200 )]
    public class Mineral : ScriptableObject, IIdentifiable
    {
        [field: SerializeField]
        public string ID { get; set; }

        public float Hardness;
        public LootTable LootTable;

        public Mesh Mesh;
        public Material Material;
    }
}