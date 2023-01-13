using DwarfMiningGame.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace DwarfMiningGame.WorldGen
{
    public class WorldGenerator
    {
        public Random rand = new Random();

        public void PlaceFeature( int startX, int startY, char[,] mask, Dictionary<char, string> key )
        {
            // mask is a 2D array of chars representing the different tiles.
            // key is a mapping from the chars to a tile ID.

            for( int i = 0; i < mask.GetLength( 1 ); i++ )
            {
                for( int j = 0; j < mask.GetLength( 0 ); j++ )
                {
                    int x = startX + i;
                    int y = startY + j;
                    if( x < 0 || x >= TileMap.Width || y < 0 || x >= TileMap.Height )
                    {
                        continue;
                    }

                    char chr = mask[j, i];
                    string tileID = key[chr];

                    // place tile at x, y.
                }
            }
        }

        public void PlaceVein( int startX, int startY, int steps, Func<Tile, (Tile, Mineral)> tileSelector, Func<float, float> radiusFunc, Func<float, float> chanceFunc, float closeness = 1.125f )
        {
            // Makes a series of spheres that intersect with each other.
            // radiusFunc takes in [0 to 1], 0 being at the beginning and 1 at the end, and spits out [0 to 1].
            // chanceFunc takes in [0 to 1], 0 being closer to the center, and spits out [0 to 1].
            for( int currStep = 0; currStep < steps; currStep++ )
            {
                float currentRadius = radiusFunc( (float)currStep / (float)steps ); // radius of current sphere, as per radius func.

                int minX = Mathf.Clamp( Mathf.FloorToInt( startX - currentRadius ), 0, TileMap.Width );
                int maxX = Mathf.Clamp( Mathf.FloorToInt( startX + currentRadius ), 0, TileMap.Width );
                int minY = Mathf.Clamp( Mathf.FloorToInt( startY - currentRadius ), 0, TileMap.Height );
                int maxY = Mathf.Clamp( Mathf.FloorToInt( startY + currentRadius ), 0, TileMap.Height );

                for( int x = minX; x < maxX; x++ )
                {
                    for( int y = minY; y < maxY; y++ )
                    {
                        float dist = Vector2.Distance( new Vector2( x, y ), new Vector2( startX, startY ) );

                        if( dist > currentRadius )
                        {
                            continue;
                        }

                        // chance for block spawn is proportional to the chanceFunc.
                        int rnd = rand.Next( 0, 1000 );
                        int thr = Mathf.FloorToInt( chanceFunc( dist / currentRadius ) * 1000.0f );

                        if( rnd < thr )
                        {
                            TileBehaviour t = TileMap.GetTile( x, y );

                            Tile originalTile = t.OriginalTile;

                            (Tile tile, Mineral min) = tileSelector( originalTile );
                            if( t != null )
                            {
                                t.Kill( true );
                            }

                            if( tile == null && min == null ) // tile null, mineral null - set to air.
                            {
                                continue;
                            }
                            // tile not specified, mineral specified - add just the mineral.
                            if( tile == null && min != null )
                            {
                                TileMap.SetTile( x, y, TileBehaviour.Create( originalTile, min ) );
                                continue;
                            }
                            // tile specified - replace tile, possibly mineral too.
                            if( tile != null )
                            {
                                TileMap.SetTile( x, y, TileBehaviour.Create( tile, min ) );
                                continue;
                            }
                            // no way to set tile and preserve mineral.
                        }
                    }
                }

                // random on circle (from polar coords with radius = radius * closeness)
                float randomAngle = (float)(rand.NextDouble() * Math.PI);
                float randomDistance = currentRadius * closeness;

                startX = (int)(randomDistance * Mathf.Cos( randomAngle )) + startX;
                startY = (int)(randomDistance * Mathf.Sin( randomAngle )) + startY;
            }
        }

        private Tile GetRandom( string baseId, int numVariants )
        {
            int r = rand.Next( 0, numVariants );

            string var = "";

            if( r > 0 )
            {
                var = $".{(r + 1)}";
            }

            return Registry<Tile>.Get( $"{baseId}{var}" );
        }

        public void Run()
        {
            for( int x = 0; x < TileMap.Width; x++ )
            {
                for( int y = 0; y < TileMap.Height; y++ )
                {
                    int yJitter = y + rand.Next( -50, 51 );

                    float depth = (float)yJitter / (float)TileMap.Height;
                    bool chance = depth > 0.5f;

                    int ironRand = rand.Next( 0, 20 );

                    if( chance )
                    {
                        TileMap.SetTile( x, y, TileBehaviour.Create( GetRandom( "tile.dirt", 3 ) ) );
                    }
                    else
                    {
                        TileMap.SetTile( x, y, TileBehaviour.Create( GetRandom( "tile.stone", 3 ) ) );
                    }
                }
            }

            for( int i = 0; i < (TileMap.Width + TileMap.Height) / 5; i++ )
            {
                int x = rand.Next( 0, TileMap.Width );
                int y = rand.Next( 0, TileMap.Height );

                PlaceVein( x, y, 25, ( t ) => (GetRandom( "tile.dirt", 3 ), null), ( n ) => (1 - n) * 2.5f, ( n ) => 0.75f );
            }

            for( int i = 0; i < (TileMap.Width + TileMap.Height) / 5; i++ )
            {
                int x = rand.Next( 0, TileMap.Width );
                int y = rand.Next( 0, TileMap.Height );

                PlaceVein( x, y, 20, ( t ) => (GetRandom( "tile.stone", 3 ), null), ( n ) => (1 - n) * 2.5f, ( n ) => 0.75f );
            }

            for( int i = 0; i < (TileMap.Width + TileMap.Height) / 5; i++ )
            {
                int x = rand.Next( 0, TileMap.Width );
                int y = rand.Next( 0, TileMap.Height );

                PlaceVein( x, y, 30, ( t ) => (null, null), ( n ) => (1 - n) * 3.5f, ( n ) => 1.0f );
            }

            for( int i = 0; i < (TileMap.Width + TileMap.Height) / 5; i++ )
            {
                int x = rand.Next( 0, TileMap.Width );
                int y = rand.Next( 0, TileMap.Height );

                PlaceVein( x, y, 3, ( t ) => (null, t == null ? null : Registry<Mineral>.Get( "mineral.iron" )), ( n ) => (1 - n) * 2.5f, ( n ) => 0.5f );
            }
        }
    }
}