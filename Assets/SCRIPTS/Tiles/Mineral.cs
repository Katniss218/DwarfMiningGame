using DwarfMiningGame.Drops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Tiles
{
    public class Mineral : MonoBehaviour, IHardnessModifier
    {
        // resources contain type and amount
        // resources are embedded in a tile
        // resources might affect tile's time to break

        [field: SerializeField]
        public float Hardness { get; set; }

        public static void Create( GameObject tile, MineralData data )
        {
            GameObject gameObject = new GameObject( "Mineral" );
            gameObject.transform.SetParent( tile.transform );
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = Vector3.one;

            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = data.Mesh;

            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = data.Material;

            Mineral min = tile.AddComponent<Mineral>();
            min.Hardness = data.Hardness;

            LootDropper ld = tile.AddComponent<LootDropper>();
            ld.Loot = data.LootTable;
        }
    }
}