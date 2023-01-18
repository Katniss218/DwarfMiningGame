using DwarfMiningGame.Drops;
using DwarfMiningGame.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.Tiles
{
    public class TileMap
    {
        private class TileInfo
        {
            public GameObject gameObject;

            public Tile tile;
            public Mineral mineral;

            public float health = 1;

            public static TileInfo Air
            {
                get
                {
                    return new TileInfo()
                    {

                    };
                }
            }

            public bool IsAir()
            {
                return this.tile == null;
            }

            public float GetTotalHardness()
            {
                float totalHardness = 0;

                if( tile != null )
                {
                    totalHardness += tile.Hardness;
                }
                if( mineral != null )
                {
                    totalHardness += mineral.Hardness;
                }

                return totalHardness;
            }

            public void KillWorld()
            {
                if( this.gameObject != null )
                {
                    GameObject.Destroy( gameObject );
                }
            }
        }

        private static TileInfo[,] _tiles;

        public static int Width { get => _tiles.GetLength( 0 ); }

        public static int Height { get => _tiles.GetLength( 1 ); }


        private static bool IsOutsideMap( int x, int y )
        {
            return (x < 0 || x >= Width) || (y < 0 || y >= Height);
        }

        private static float GetDamage( in float miningSpeed, in float hardness )
        {
            return (miningSpeed / hardness) * Time.deltaTime;
        }

        /// <summary>
        /// Hurts the tile.
        /// </summary>
        public static void Mine( int x, int y, float maxHardness, float miningSpeed, bool silent = false )
        {
            if( IsOutsideMap( x, y ) )
            {
                return;
            }

            TileInfo info = _tiles[x, y];

            if( info.IsAir() )
            {
                return;
            }

            if( info.tile.Hardness > maxHardness )
            {
                return;
            }

            info.health -= GetDamage( miningSpeed, info.GetTotalHardness() );

            if( info.health <= 0 )
            {
                Kill( x, y, silent );
            }
        }

        /// <summary>
        /// Removes the tile, drops the contents.
        /// </summary>
        public static void Kill( int x, int y, bool silent = false )
        {
            if( IsOutsideMap( x, y ) )
            {
                return;
            }

            TileInfo info = _tiles[x, y];

            if( info.IsAir() )
            {
                return;
            }

            info.KillWorld();

            _tiles[x, y] = TileInfo.Air;

            if( !silent )
            {
                Vector3 worldPos = GetWorldPosition( x, y );

                List<(Item, int)> dropped = new List<(Item, int)>();

                if( info.tile.LootTable != null )
                {
                    dropped.AddRange( info.tile.LootTable.Generate() );
                }

                if( info.mineral != null && info.mineral.LootTable != null )
                {
                    dropped.AddRange( info.mineral.LootTable.Generate() );
                }

                foreach( (Item item, int amt) in dropped )
                {
                    for( int i = 0; i < amt; i++ )
                    {
                        DroppedItem.Create( worldPos, item, 1 );
                    }
                }
            }
        }

        /// <summary>
        /// Tiles start in the lower-left corner, and increase to upper right.
        /// </summary>
        public static Vector3 GetWorldPosition( int x, int y )
        {
            return new Vector3( x, y, 0 );
        }

        /// <summary>
        /// Tiles start in the lower-left corner, and increase to upper right.
        /// </summary>
        public static (int x, int y) GetTilePosition( Vector3 position )
        {
            return (Mathf.FloorToInt( position.x + 0.5f ), Mathf.FloorToInt( position.y + 0.5f ));
        }

        public static int GetTop( int x )
        {
            for( int j = Height - 1; j >= 0; j-- )
            {
                if( !_tiles[x, j].IsAir() )
                {
                    return j;
                }
            }
            return 0;
        }

        public static (Tile, Mineral) GetTile( int x, int y )
        {
            if( IsOutsideMap( x, y ) )
            {
                return (null, null);
            }

            TileInfo info = _tiles[x, y];
            return (info.tile, info.mineral);
        }

        public static void SetTile( int x, int y, Tile tile, Mineral mineral )
        {
            if( tile == null )
            {
                throw new ArgumentNullException( "Tile must be set" );
            }

            if( IsOutsideMap( x, y ) )
            {
                throw new InvalidOperationException( "Can't set a tile outside bounds" );
            }

            TileInfo existingInfo = _tiles[x, y];
            if( !existingInfo.IsAir() )
            {
                Kill( x, y, true );
            }

            GameObject tileObject = Tile.Create( tile, mineral );
            tileObject.isStatic = true;
            tileObject.transform.position = GetWorldPosition( x, y );

            _tiles[x, y] = new TileInfo() { gameObject = tileObject, tile = tile, mineral = mineral };
        }

        public static void CreateMap( int width, int height )
        {
            _tiles = new TileInfo[width, height];

            for( int x = 0; x < width; x++ )
            {
                for( int y = 0; y < height; y++ )
                {
                    _tiles[x, y] = new TileInfo();
                }
            }
        }
    }
}