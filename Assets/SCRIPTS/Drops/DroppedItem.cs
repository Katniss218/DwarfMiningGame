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
            if( amt <= 0 )
            {
                Destroy( this.gameObject );
            }
        }

        public static DroppedItem Create( Vector3 position, Item item, int amt )
        {
            GameObject go = new GameObject( "Item" );
            go.transform.position = position;

            SphereCollider c = go.AddComponent<SphereCollider>();
            c.radius = 0.25f;

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