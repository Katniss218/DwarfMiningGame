using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Player
{
    [RequireComponent( typeof( Rigidbody ) )]
    public class PlayerController : MonoBehaviour
    {
        Rigidbody _rigidbody;

        [field: SerializeField]
        public float MoveForce { get; set; }

        [field: SerializeField]
        public float LiftForce { get; set; }

        void Awake()
        {
            _rigidbody = this.GetComponent<Rigidbody>();
        }

        // Update is called once per frame
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