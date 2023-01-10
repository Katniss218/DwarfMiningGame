using DwarfMiningGame.Tiles;
using DwarfMiningGame.WorldGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame
{
    public class GameManager : MonoBehaviour
    {
        void Awake()
        {
            TileMap.CreateMap( 200, 200 );

            new WorldGenerator().Run();
        }
    }
}
