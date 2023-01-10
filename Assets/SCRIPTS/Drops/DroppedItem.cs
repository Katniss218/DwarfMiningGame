using DwarfMiningGame.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.Drops
{
    [RequireComponent( typeof( Collider ) )]
    public class DroppedItem : MonoBehaviour
    {
        public Item Item { get; set; }
        public int Amt { get; set; }

        public void OnTake( int amt )
        {
            this.Amt -= amt;
            if( this.Amt <= 0 )
            {
                Destroy( this.gameObject );
            }
        }

        const int LAYER = 10;

        public static DroppedItem Create( Vector3 position, Item item, int amt )
        {
            GameObject go = new GameObject( "Item" );
            go.layer = LAYER;
            go.transform.position = position + new Vector3( UnityEngine.Random.Range( -0.125f, 0.125f ), UnityEngine.Random.Range( -0.125f, 0.125f ), 0.0f );

            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezePositionZ;

            SphereCollider c = go.AddComponent<SphereCollider>();
            c.radius = 0.25f;
           // c.isTrigger = true;

            MeshFilter mf = go.AddComponent<MeshFilter>();
            mf.mesh = item.Mesh;

            MeshRenderer mr = go.AddComponent<MeshRenderer>();
            mr.material = item.Material;

            DroppedItem di = go.AddComponent<DroppedItem>();
            di.Item = item;
            di.Amt = amt;

            return di;
        }
    }
}