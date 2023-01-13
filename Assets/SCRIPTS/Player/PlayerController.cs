using DwarfMiningGame.Items;
using DwarfMiningGame.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Player
{
    [RequireComponent( typeof( Rigidbody ) )]
    [RequireComponent( typeof( Collider ) )]
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField]
        public float MoveForce { get; set; }

        [field: SerializeField]
        public float LiftForce { get; set; }

        Rigidbody _rigidbody;
        Collider _collider;
        PlayerInventory _inventory;

        [field: SerializeField]
        public bool IsOnGround { get; private set; }

        void Awake()
        {
            _rigidbody = this.GetComponent<Rigidbody>();
            _collider = this.GetComponent<Collider>();
            _inventory = this.GetComponent<PlayerInventory>();
        }

        void UsePickaxe()
        {
            ItemPickaxe pickaxe = _inventory.Pickaxe;
            if( pickaxe == null )
            {
                return;
            }

            int dirX = 0;
            int dirY = 0;

            if( Input.GetKey( KeyCode.S ) ) // pressing S - mine down no matter what.
            {
                dirY = -1;
            }
            else if( IsOnGround ) // not pressing S - mine sideways only if on ground.
            {
                dirX = this.transform.forward.x < 0 ? -1 : 1;
            }
            else
            {
                return;
            }

            (int x, int y) = TileMap.GetTilePosition( this.transform.position );

            TileBehaviour tile = TileMap.GetTile( x + dirX, y + dirY );
            if( tile == null )
            {
                return;
            }
            tile.Mine( pickaxe.MiningPower );
        }

        void Update()
        {
            if( Input.GetKey( KeyCode.Mouse0 ) )
            {
                UsePickaxe();
            }
        }

        void FixedUpdate()
        {
            if( Physics.Raycast( this.transform.position, Vector3.down, this._collider.bounds.extents.y + 0.05f, 1 << TileBehaviour.LAYER ) )
            {
                IsOnGround = true;
            }
            else
            {
                IsOnGround = false;
            }

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
                IsOnGround = false;
                totalForce += new Vector3( 0.0f, LiftForce, 0.0f );
            }
            if( Input.GetKey( KeyCode.S ) )
            {
                totalForce += new Vector3( 0.0f, -LiftForce, 0.0f );
            }

            if( totalForce.x < 0 )
            {
                this.transform.forward = Vector3.left;
            }
            else if( totalForce.x > 0 )
            {
                this.transform.forward = Vector3.right;
            }

            this._rigidbody.AddForce( totalForce );
        }
    }
}