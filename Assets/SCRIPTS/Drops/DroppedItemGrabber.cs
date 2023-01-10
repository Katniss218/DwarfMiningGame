using DwarfMiningGame.Items;
using UnityEngine;


namespace DwarfMiningGame.Drops
{
    [RequireComponent( typeof( Inventory ) )]
    public class DroppedItemGrabber : MonoBehaviour
    {
        Inventory _inventory;

        void Awake()
        {
            _inventory = this.GetComponent<Inventory>();
        }

        // require inventory.
        // pick up every droppeditem you can.
        void Update()
        {
            Collider[] cols = Physics.OverlapSphere( this.transform.position, 2.0f );
            foreach( Collider col in cols )
            {
                DroppedItem stack = col.GetComponent<DroppedItem>();
                if( stack == null )
                {
                    continue;
                }

                PickUp( stack );
            }
        }


        private void PickUp( DroppedItem stack )
        {
            int amtPickedUp = _inventory.Add( stack.Item, stack.Amt );
            stack.OnTake( amtPickedUp );
        }
    }
}
