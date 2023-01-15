﻿using DwarfMiningGame.Tiles;
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
        [SerializeField] GameObject _player;
        [SerializeField] GameObject _background;

        [SerializeField] int width;
        [SerializeField] int height;

        void Awake()
        {
            TileMap.CreateMap( width, height );

            new WorldGenerator().Run();

            _player.transform.position = TileMap.GetWorldPosition( width / 2, TileMap.GetTop( width / 2 ) + 1 );

            if( _background != null )
            {
                _background.transform.localScale = new Vector3( width, height, 1.0f );
                _background.transform.position = TileMap.GetWorldPosition( width / 2, height / 2 ) + new Vector3( -0.5f, -0.5f, 1.0f );
            }
        }
    }
}
