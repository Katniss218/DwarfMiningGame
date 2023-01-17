﻿using DwarfMiningGame.Items;
using DwarfMiningGame.Tiles;
using DwarfMiningGame.UI;
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
        [SerializeField] PlayerInventory _playerInventory;
        [SerializeField] GameObject _background;

        [SerializeField] int width;
        [SerializeField] int height;

        [SerializeField] bool randomSeed;
        [SerializeField] int seed;

        private static RectTransform _mainCanvas;
        public static RectTransform MainCanvas
        {
            get
            {
                if( _mainCanvas == null )
                {
                    _mainCanvas = GameObject.Find( "Main Canvas" ).GetComponent<RectTransform>();
                }
                return _mainCanvas;
            }
        }

        private static RectTransform _contextMenuCanvas;
        public static RectTransform ContextMenuCanvas
        {
            get
            {
                if( _contextMenuCanvas == null )
                {
                    _contextMenuCanvas = GameObject.Find( "Context Menu Canvas" ).GetComponent<RectTransform>();
                }
                return _contextMenuCanvas;
            }
        }

        void Awake()
        {
            TileMap.CreateMap( width, height );

            if( randomSeed )
            {
                seed = new System.Random().Next();
            }

            new WorldGenerator( seed ).Run();

            _player.transform.position = TileMap.GetWorldPosition( width / 2, TileMap.GetTop( width / 2 ) + 1 );

            if( _background != null )
            {
                _background.transform.localScale = new Vector3( width, height, 1.0f );
                _background.transform.position = TileMap.GetWorldPosition( width / 2, height / 2 ) + new Vector3( -0.5f, -0.5f, 1.0f );
            }
        }

        private void Start()
        {
            _playerInventory.TryAdd( Registry<Item>.Get( "item.dwarven_pickaxe" ), 1 );
            _playerInventory.Money = 200.0f;

            PlayerInventoryUI.Create( MainCanvas, _playerInventory );
        }
    }
}
