﻿using DwarfMiningGame.Drops;
using DwarfMiningGame.Loot;
using UnityEngine;

namespace DwarfMiningGame.Tiles
{

    /// <summary>
    /// The base parameters of a tile. Each tile has those.
    /// </summary>
    [RequireComponent( typeof( BoxCollider ) )]
    public class TileBehaviour : MonoBehaviour
    {
        // Tiles can contain resources
        // They can contain a type and amount of resource. Different amounts are displayed differently (bigger veins).
        // They take time to break, time can be dependant on the type of tile and resource inside.

        public string OriginalTileID { get; private set; } = "invalid";

        /// <summary>
        /// Describes how hard it is to break the tile.
        /// </summary>
        [field: SerializeField]
        public float BaseHardness { get; set; }

        public float GetTotalHardness()
        {
            IHardnessModifier[] hardnessModifiers = this.GetComponents<IHardnessModifier>();

            float hardnessTotal = this.BaseHardness;
            foreach( var mod in hardnessModifiers )
            {
                hardnessTotal += mod.Hardness;
            }

            return hardnessTotal;
        }

        // Health - (0 to 1), decreases based on hardness.
        [SerializeField]
        private float _health = 1.0f;

        [SerializeField]
        private float _maxHealth = 1.0f;

        BoxCollider _collider;

        void Start()
        {
            this._collider = this.GetComponent<BoxCollider>();
        }

        void Update()
        {

        }

        private static float GetDamage( in float miningPower, in float hardness )
        {
            return miningPower / hardness;
        }

        /// <summary>
        /// Hurts the tile.
        /// </summary>
        public void Mine( float miningPower, bool silent = false )
        {
            this._health -= GetDamage( miningPower, this.GetTotalHardness() );

            if( this._health <= 0 )
            {
                this.Kill( silent );
            }
        }

        /// <summary>
        /// Removes the tile, drops the contents.
        /// </summary>
        public void Kill( bool silent = false )
        {
            IKillCallback[] killables = this.GetComponents<IKillCallback>();
            foreach( var k in killables )
            {
                k.OnKill( silent );
            }
            Destroy( this.gameObject );
        }

        const int LAYER = 9;

        /// <summary>
        /// Creates a new tile prefab.
        /// </summary>
        public static TileBehaviour Create( Tile data, Mineral mineral = null )
        {
            GameObject gameObject = new GameObject( "TILE" );
            gameObject.layer = LAYER;

            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3( 1.0f, 1.0f, 1.0f );

            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = data.Mesh;

            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = data.Material;

            TileBehaviour tile = gameObject.AddComponent<TileBehaviour>();
            tile.BaseHardness = data.Hardness;
            tile._maxHealth = 1.0f;
            tile._health = tile._maxHealth;
            tile.OriginalTileID = data.ID;

            LootDropper ld = gameObject.AddComponent<LootDropper>();
            ld.Loot = data.LootTable;

            if( mineral != null )
            {
                MineralBehaviour.Create( gameObject, mineral );
            }

            return tile;
        }
    }
}