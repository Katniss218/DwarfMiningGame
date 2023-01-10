using DwarfMiningGame.Items;
using DwarfMiningGame.Loot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.Drops
{
    public class LootDropper : MonoBehaviour
    {
        [field: SerializeField]
        public LootTable Loot { get; set; }

        void OnDestroy()
        {
            if( !this.gameObject.scene.isLoaded ) // player closed.
            {
                return;
            }

            List<(Item, int)> dropped = Loot.Generate();

            foreach( (Item item, int amt) in dropped )
            {
                for( int i = 0; i < amt; i++ )
                {
                    DroppedItem.Create( this.transform.position, item, 1 );
                }
            }
        }
    }
}