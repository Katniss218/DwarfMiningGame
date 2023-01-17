using DwarfMiningGame.Items;
using DwarfMiningGame.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DwarfMiningGame.Player
{
    [RequireComponent( typeof( Rigidbody ) )]
    [RequireComponent( typeof( Collider ) )]
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField]
        public float MoveForce { get; set; }

        [field: SerializeField]
        public float JumpForce { get; set; }

        Rigidbody _rigidbody;
        Collider _collider;
        PlayerInventory _inventory;

        public bool IsOnGround { get; private set; }

        public bool IsTouchingAWall { get; private set; }

        void Awake()
        {
            _rigidbody = this.GetComponent<Rigidbody>();
            _collider = this.GetComponent<Collider>();
            _inventory = this.GetComponent<PlayerInventory>();
        }

        void UsePickaxe()
        {
            // Hand item can be not set, or set to a slot that was later removed (item was sold/etc).
            ItemPickaxe pickaxe = _inventory.Hand?.Item as ItemPickaxe;
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

            TileMap.Mine( x + dirX, y + dirY, pickaxe.MaxHardness, pickaxe.MiningSpeed );
        }

        bool StandingOnTile()
        {
            return Physics.Raycast( this.transform.position, Vector3.down, this._collider.bounds.extents.y + 0.05f, 1 << Tile.LAYER );
        }

        bool TouchingTile( Vector3 dir )
        {
            return Physics.Raycast( this.transform.position, dir, this._collider.bounds.extents.y + 0.015f, 1 << Tile.LAYER );
        }

        bool VelocityCanWallJump()
        {
            return this._rigidbody.velocity.y < -0.5f && this._rigidbody.velocity.y > -2.5f;
        }

        void ResetVelocityX()
        {
            this._rigidbody.velocity = new Vector3( 0.0f, this._rigidbody.velocity.y, this._rigidbody.velocity.z );
        }

        void ResetVelocityY()
        {
            this._rigidbody.velocity = new Vector3( this._rigidbody.velocity.x, 0.0f, this._rigidbody.velocity.z );
        }

        void Update()
        {
            if( !EventSystem.current.IsPointerOverGameObject() && Input.GetKey( KeyCode.Mouse0 ) )
            {
                UsePickaxe();
            }
        }

        void FixedUpdate()
        {
            IsOnGround = false;
            IsTouchingAWall = false;

            if( Input.GetKey( KeyCode.A ) )
            {
                this.transform.forward = Vector3.left;
            }
            else if( Input.GetKey( KeyCode.D ) )
            {
                this.transform.forward = Vector3.right;
            }

            if( StandingOnTile() )
            {
                ResetVelocityY();
                IsOnGround = true;
            }

            // clamp Y
            if( this.transform.position.y < 0.0f )
            {
                ResetVelocityY();
                this.transform.position = new Vector3( this.transform.position.x, 0.0f, this.transform.position.z );
                IsOnGround = true;
            }

            Vector3 totalForce = Vector3.zero;

            if( TouchingTile( Vector3.left ) && VelocityCanWallJump() )
            {
                ResetVelocityX();
                IsTouchingAWall = true;
            }
            else
            {
                if( Input.GetKey( KeyCode.A ) )
                {
                    totalForce += new Vector3( -MoveForce, 0.0f, 0.0f );
                }
            }

            if( TouchingTile( Vector3.right ) && VelocityCanWallJump() )
            {
                ResetVelocityX();
                IsTouchingAWall = true;
            }
            else
            {
                if( Input.GetKey( KeyCode.D ) )
                {
                    totalForce += new Vector3( MoveForce, 0.0f, 0.0f );
                }
            }

            if( Input.GetKey( KeyCode.Space ) || Input.GetKey( KeyCode.W ) )
            {
                if( IsOnGround || IsTouchingAWall )
                {
                    this._rigidbody.AddForce( new Vector3( 0.0f, JumpForce, 0.0f ), ForceMode.Impulse );
                }
                IsOnGround = false;
            }

            this._rigidbody.AddForce( totalForce );

            // clamp X
            if( this.transform.position.x < 0.0f )
            {
                this.transform.position = new Vector3( 0.0f, this.transform.position.y, this.transform.position.z );
                ResetVelocityX();
            }
            else if( this.transform.position.x > ((float)TileMap.Width - 1.0f) )
            {
                this.transform.position = new Vector3( ((float)TileMap.Width - 1.0f), this.transform.position.y, this.transform.position.z );
                ResetVelocityX();
            }
        }
    }
}