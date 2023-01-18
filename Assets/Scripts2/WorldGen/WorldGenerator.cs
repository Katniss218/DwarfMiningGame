using DwarfMiningGame.Tiles;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace DwarfMiningGame.WorldGen
{
    public class WorldGenerator
    {
        public Random rand;

        public WorldGenerator( int seed )
        {
            rand = new Random( seed );
        }

        public void PlaceFeature( int startX, int startY, char[,] mask, Dictionary<char, (Tile, Mineral)> key )
        {
            // mask is a 2D array of chars representing the different tiles.
            // key is a mapping from the chars to a tile ID.

            int w = mask.GetLength( 0 );
            int h = mask.GetLength( 1 );
            for( int j = 0; j < h; j++ )
            {
                for( int i = 0; i < w; i++ )
                {
                    int x = startX + i;
                    int y = startY + (w - j);
                    if( x < 0 || x >= TileMap.Width || y < 0 || y >= TileMap.Height )
                    {
                        continue;
                    }

                    char chr = mask[j, i];
                    (Tile tile, Mineral mineral) = key[chr];

                    SetTile( x, y, tile, mineral );
                }
            }
        }

        public void SetTile( int x, int y, Tile newTile, Mineral newMineral )
        {
            (Tile originalTile, _) = TileMap.GetTile( x, y );

            if( newTile == null && newMineral == null ) // tile null, mineral null - set to air.
            {
                TileMap.Kill( x, y, true );
                return;
            }
            // tile not specified, mineral specified - add just the mineral.
            if( newTile == null && newMineral != null )
            {
                TileMap.SetTile( x, y, originalTile, newMineral );
                return;
            }
            // tile specified - replace tile, possibly mineral too.
            if( newTile != null )
            {
                TileMap.SetTile( x, y, newTile, newMineral );
                return;
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
                        int random = rand.Next( 0, 1000 );
                        int threshold = Mathf.FloorToInt( chanceFunc( dist / currentRadius ) * 1000.0f );

                        if( random < threshold )
                        {
                            (Tile originalTile, _) = TileMap.GetTile( x, y );

                            (Tile newTile, Mineral newMineral) = tileSelector( originalTile );

                            SetTile( x, y, newTile, newMineral );
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

        private void OreRunner( int minX, int maxX, int minY, int maxY, float blockChance, Action<int, int> callee )
        {
            float area = (maxX - minX) * (maxY - minY);
            int numCalls = Mathf.FloorToInt( area * blockChance );

            for( int i = 0; i < numCalls; i++ )
            {
                int x = rand.Next( minX, maxX + 1 );
                int y = rand.Next( minY, maxY + 1 );

                callee.Invoke( x, y );
            }
        }

        public void Run()
        {
            int terrainHeight = Mathf.FloorToInt( TileMap.Height * 0.9f );

            for( int x = 0; x < TileMap.Width; x++ )
            {
                for( int y = 0; y < terrainHeight; y++ )
                {
                    int yJitter = y + rand.Next( -50, 51 );

                    float depth = (float)yJitter / (float)TileMap.Height;
                    bool chance = depth > 0.5f;

                    int ironRand = rand.Next( 0, 20 );

                    if( chance )
                    {
                        TileMap.SetTile( x, y, GetRandom( "tile.dirt", 3 ), null );
                    }
                    else
                    {
                        TileMap.SetTile( x, y, GetRandom( "tile.stone", 3 ), null );
                    }
                }
            }

            // surface.
            for( int i = 0; i < TileMap.Width / 10; i++ )
            {
                int x = rand.Next( 0, TileMap.Width );
                int y = rand.Next( terrainHeight - 3, terrainHeight + 3 );

                PlaceVein( x, y, 4, ( t ) => (GetRandom( "tile.dirt", 3 ), null), ( n ) => Mathf.Lerp( 5f, 10f, n ), ( n ) => 1.0f );
            }
            for( int x = 0; x < TileMap.Width; x++ )
            {
                for( int y = terrainHeight; y < TileMap.Height; y++ )
                {
                    (Tile t, _) = TileMap.GetTile( x, y );
                    if( t == null )
                    {
                        continue;
                    }

                    if( y == TileMap.Height - 1 )
                    {
                        TileMap.SetTile( x, y, GetRandom( "tile.grass", 3 ), null );
                        continue;
                    }
                    (Tile t2, _) = TileMap.GetTile( x, y + 1 );
                    if( t2 == null )
                    {
                        TileMap.SetTile( x, y, GetRandom( "tile.grass", 3 ), null );
                    }
                }
            }

            for( int i = 0; i < (TileMap.Width + TileMap.Height) / 5; i++ )
            {
                int x = rand.Next( 0, TileMap.Width );
                int y = rand.Next( 0, terrainHeight );

                PlaceVein( x, y, 25, ( t ) => (GetRandom( "tile.dirt", 3 ), null), ( n ) => (1 - n) * 2.5f, ( n ) => 0.75f );
            }

            for( int i = 0; i < (TileMap.Width + TileMap.Height) / 5; i++ )
            {
                int x = rand.Next( 0, TileMap.Width );
                int y = rand.Next( 0, terrainHeight );

                PlaceVein( x, y, 20, ( t ) => (GetRandom( "tile.stone", 3 ), null), ( n ) => (1 - n) * 2.5f, ( n ) => 0.75f );
            }

            // caves
            for( int i = 0; i < (TileMap.Width + TileMap.Height) / 7; i++ )
            {
                int x = rand.Next( 0, TileMap.Width );
                int y = rand.Next( 0, TileMap.Height );

                PlaceVein( x, y, 25, ( t ) => (null, null), ( n ) => Mathf.Lerp( 0.25f, 3.5f, n ), ( n ) => 1.0f );
            }
            for( int i = 0; i < (TileMap.Width + TileMap.Height) / 5; i++ )
            {
                int x = rand.Next( 0, TileMap.Width );
                int y = rand.Next( 0, TileMap.Height );

                PlaceVein( x, y, 3, ( t ) => (null, null), ( n ) => Mathf.Lerp( 1.5f, 3f, n ), ( n ) => 1.0f );
            }
            for( int i = 0; i < (TileMap.Width + TileMap.Height) / 20; i++ )
            {
                int x = rand.Next( 0, TileMap.Width );
                int y = rand.Next( 0, TileMap.Height );

                PlaceVein( x, y, 2, ( t ) => (null, null), ( n ) => Mathf.Lerp( 3.75f, 5.0f, n ), ( n ) => 1.0f );
            }

            OreRunner( 0, TileMap.Width - 1, Mathf.RoundToInt( TileMap.Height * 0.85f ), Mathf.RoundToInt( TileMap.Height * 1.0f ), 0.05f, ( x, y ) =>
            {
                PlaceVein( x, y, 3, ( t ) => (null, t == null ? null : Registry<Mineral>.Get( "mineral.coal" )), ( n ) => (1 - n) * 1.25f, ( n ) => 0.45f );
            } );
            OreRunner( 0, TileMap.Width - 1, Mathf.RoundToInt( TileMap.Height * 0.5f ), Mathf.RoundToInt( TileMap.Height * 1.0f ), 0.045f, ( x, y ) =>
            {
                PlaceVein( x, y, 2, ( t ) => (null, t == null ? null : Registry<Mineral>.Get( "mineral.copper" )), ( n ) => (1 - n) * 1.25f, ( n ) => 0.5f );
            } );
            OreRunner( 0, TileMap.Width - 1, Mathf.RoundToInt( TileMap.Height * 0.2f ), Mathf.RoundToInt( TileMap.Height * 1.0f ), 0.045f, ( x, y ) =>
            {
                PlaceVein( x, y, 2, ( t ) => (null, t == null ? null : Registry<Mineral>.Get( "mineral.iron" )), ( n ) => (1 - n) * 1.25f, ( n ) => 0.5f );
            } );
            OreRunner( 0, TileMap.Width - 1, Mathf.RoundToInt( TileMap.Height * 0.1f ), Mathf.RoundToInt( TileMap.Height * 0.9f ), 0.025f, ( x, y ) =>
            {
                PlaceVein( x, y, 2, ( t ) => (null, t == null ? null : Registry<Mineral>.Get( "mineral.silver" )), ( n ) => (1 - n) * 1.25f, ( n ) => 0.5f );
            } );
            OreRunner( 0, TileMap.Width - 1, Mathf.RoundToInt( TileMap.Height * 0.0f ), Mathf.RoundToInt( TileMap.Height * 0.55f ), 0.025f, ( x, y ) =>
            {
                PlaceVein( x, y, 2, ( t ) => (null, t == null ? null : Registry<Mineral>.Get( "mineral.silver" )), ( n ) => (1 - n) * 1.25f, ( n ) => 0.5f );
            } );

            char[,] secret3x3 = new char[,]
            {
                { ' ', ' ', ' ', ' ', ' ' },
                { ' ', 'X', 'X', 'X', ' ' },
                { ' ', 'X', 'X', 'X', ' ' },
                { ' ', 'X', 'X', 'X', ' ' },
                { ' ', ' ', ' ', ' ', ' ' }
            };
            Dictionary<char, (Tile, Mineral)> goldSecretDict = new Dictionary<char, (Tile, Mineral)>()
            {
                { ' ', (null, null) },
                { 'X', (Registry<Tile>.Get( "tile.stone.3" ), Registry<Mineral>.Get( "mineral.gold" )) }
            };
            Dictionary<char, (Tile, Mineral)> silverSecretDict = new Dictionary<char, (Tile, Mineral)>()
            {
                { ' ', (null, null) },
                { 'X', (Registry<Tile>.Get( "tile.stone.3" ), Registry<Mineral>.Get( "mineral.silver" )) }
            };
            Dictionary<char, (Tile, Mineral)> stonebrickDict = new Dictionary<char, (Tile, Mineral)>()
            {
                { ' ', (null, null) },
                { 'X', (Registry<Tile>.Get( "tile.stone_brick" ), null) }
            };

            for( int i = 0; i < (TileMap.Width * TileMap.Height) * 0.0001f; i++ )
            {
                int x = rand.Next( 0, TileMap.Width );
                int y = rand.Next( Mathf.RoundToInt( terrainHeight * 0.2f ), Mathf.RoundToInt( terrainHeight * 0.95f ) );

                PlaceFeature( x, y, secret3x3, silverSecretDict );
            }
            for( int i = 0; i < (TileMap.Width * TileMap.Height) * 0.0001f; i++ )
            {
                int x = rand.Next( 0, TileMap.Width );
                int y = rand.Next( Mathf.RoundToInt( terrainHeight * 0.1f ), Mathf.RoundToInt( terrainHeight * 0.6f ) );

                PlaceFeature( x, y, secret3x3, goldSecretDict );
            }


            char[,] s = new char[,]
            {
                { 'X', 'X', 'X' },
                { 'X', 'X', 'X' },
                { 'X', 'X', 'X' },
            };
            for( int i = 0; i < (TileMap.Width * TileMap.Height) * 0.0001f; i++ )
            {
                int x = rand.Next( 0, TileMap.Width );
                int y = rand.Next( Mathf.RoundToInt( terrainHeight * 0.1f ), Mathf.RoundToInt( terrainHeight * 0.6f ) );

                PlaceFeature( x, y, s, stonebrickDict );
            }
        }
    }
}