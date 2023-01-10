using DwarfMiningGame.Items;
using DwarfMiningGame.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Player
{
    [RequireComponent( typeof( Rigidbody ) )]
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField]
        public float MoveForce { get; set; }

        [field: SerializeField]
        public float LiftForce { get; set; }

        Rigidbody _rigidbody;
        PlayerInventory _inventory;

        void Awake()
        {
            _rigidbody = this.GetComponent<Rigidbody>();
            _inventory = this.GetComponent<PlayerInventory>();
        }

        void Update()
        {
            if( Input.GetKey( KeyCode.Mouse0 ) )
            {
                PickaxeItem pickaxe = _inventory.Pickaxe;

                (int x, int y) = TileMap.GetTilePosition( this.transform.position );
                Tile tile = TileMap.GetTile( x, y - 1 );
                tile.Mine( pickaxe.MiningPower );
            }
        }

        void FixedUpdate()
        {
            Vector3 totalForce = Vector3.zero;

            if( Input.GetKey( KeyCode.A ) )
            {
                totalForce += new Vector3( -MoveForce, 0.0f, 0.0f );
            }
            if( Input.GetKey( KeyCode.D ) )
            {
                totalForce += new Vector3( MoveForce, 0.0f, 0.0f );
            }

            if( Input.GetKey( KeyCode.W ) )
            {
                totalForce += new Vector3( 0.0f, LiftForce, 0.0f );
            }
            if( Input.GetKey( KeyCode.S ) )
            {
                totalForce += new Vector3( 0.0f, -LiftForce, 0.0f );
            }

            this._rigidbody.AddForce( totalForce );
        }
    }
}