using DwarfMiningGame.Loot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.Tiles
{
    [CreateAssetMenu( fileName = "tile", menuName = "DwarfMiningGame/Tile", order = 10 )]
    public class TileData : ScriptableObject, IIdentifiable
    {
        [field: SerializeField]
        public string ID { get; set; }
        public float Hardness;
        public LootTable LootTable;

        public Mesh Mesh;
        public Material Material;
    }
}
