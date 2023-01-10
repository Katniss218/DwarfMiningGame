using DwarfMiningGame.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfMiningGame.WorldGen
{
    public static class WorldGenerator
    {
        public static void Run()
        {
            for( int x = 0; x < TileMap.Width; x++ )
            {
                for( int y = 0; y < TileMap.Width; y++ )
                {
                    TileMap.SetTile( x, y, Tile.Create( GameRegistry.RegisteredTiles[0] ) );
                }
            }
        }
    }
}
