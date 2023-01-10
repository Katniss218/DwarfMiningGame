﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.Tiles
{
    public class TileMap
    {
        private static Tile[,] _tiles;

        public static int Width { get => _tiles.GetLength( 0 ); }

        public static int Height { get => _tiles.GetLength( 1 ); }

        public static Vector3 GetWorldPosition( int x, int y )
        {
            return new Vector3( x, y, 0 );
        }

        public static (int x, int y) GetTilePosition( Vector3 position )
        {
            return ((int)position.x, (int)position.y);
        }

        public static Tile GetTile( int x, int y )
        {
            return _tiles[x, y];
        }

        public static void SetTile( int x, int y, Tile tile )
        {
            tile.transform.position = GetWorldPosition( x, y );

            _tiles[x, y] = tile;
        }

        public static void CreateMap( int width, int height )
        {
            _tiles = new Tile[width, height];
        }
    }
}