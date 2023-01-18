using DwarfMiningGame.Drops;
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


        public static void Create( GameObject tile, Mineral mineral )
        {
            GameObject gameObject = new GameObject( "Mineral" );
            gameObject.transform.SetParent( tile.transform );
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = Vector3.one;

            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mineral.Mesh;

            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = mineral.Material;
        }
    }
}