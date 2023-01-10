using DwarfMiningGame.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfMiningGame.WorldGen
{
    public class WorldGenerator
    {
        public Random rand = new Random();

        public void Run()
        {
            for( int x = 0; x < TileMap.Width; x++ )
            {
                for( int y = 0; y < TileMap.Width; y++ )
                {
                    int r = rand.Next( 0, 4 );
                    int ironRand = rand.Next( 0, 20 );
                    MineralData min = null;
                    if( r == 0 || r == 1 )
                    {
                        TileMap.SetTile( x, y, Tile.Create( GameRegistry.RegisteredTiles[0] ) );
                    }
                    else if( r == 2 )
                    {
                        if( ironRand == 0 )
                        {
                            min = GameRegistry.RegisteredMinerals[0];
                        }
                        TileMap.SetTile( x, y, Tile.Create( GameRegistry.RegisteredTiles[1], min ) );
                    }
                    else
                    {
                        if( ironRand == 0 )
                        {
                            min = GameRegistry.RegisteredMinerals[0];
                        }
                        TileMap.SetTile( x, y, Tile.Create( GameRegistry.RegisteredTiles[2], min ) );
                    }
                }
            }
        }
    }
}
