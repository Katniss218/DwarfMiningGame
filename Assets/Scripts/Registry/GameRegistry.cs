using DwarfMiningGame.Items;
using DwarfMiningGame.Tiles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DwarfMiningGame
{
    public class GameRegistry : MonoBehaviour
    {
        [SerializeField]
        private MineralData[] _registeredMinerals;

        [SerializeField]
        private TileData[] _registeredTiles;

        [SerializeField]
        private Item[] _registeredItems;


        public static MineralData[] RegisteredMinerals { get => _instance._registeredMinerals; }

        public static TileData[] RegisteredTiles { get => _instance._registeredTiles; }

        public static Item[] RegisteredItems { get => _instance._registeredItems; }

        public static TileData GetTile( string id )
        {
            return RegisteredTiles.FirstOrDefault( n => n.ID == id );
        }

        public static Item GetItem( string id )
        {
            return RegisteredItems.FirstOrDefault( n => n.ID == id );
        }

        private static GameRegistry ___instance;
        private static GameRegistry _instance
        {
            get
            {
                if( ___instance == null )
                {
                    ___instance = FindObjectOfType<GameRegistry>();
                    if( ___instance == null )
                    {
                        throw new InvalidOperationException( "There was no GameRegistry in the scene" );
                    }
                }
                return ___instance;
            }
        }
    }
}