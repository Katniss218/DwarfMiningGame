using DwarfMiningGame.Loot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.Tiles
{
    [CreateAssetMenu( fileName = "tile", menuName = "DwarfMiningGame/Tile", order = 100 )]
    public class Tile : ScriptableObject, IIdentifiable
    {
        [field: SerializeField]
        public string ID { get; set; }
        public float Hardness;
        public LootTable LootTable;

        public Mesh Mesh;
        public Material Material;


        public const int LAYER = 9;

        /// <summary>
        /// Creates a new tile prefab.
        /// </summary>
        public static GameObject Create( Tile tile, Mineral mineral = null )
        {
            if( tile == null )
            {
                throw new ArgumentNullException( "Tile must be set" );
            }

            GameObject gameObject = new GameObject( "TILE" );
            gameObject.layer = LAYER;

            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3( 1.0f, 1.0f, 1.0f );

            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = tile.Mesh;

            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = tile.Material;

            if( mineral != null )
            {
                Mineral.Create( gameObject, mineral );
            }
            
            return gameObject;
        }
    }
}
