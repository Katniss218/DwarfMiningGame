using DwarfMiningGame.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.Loot
{
    /// Stores loot to be dropped by something.
    [Serializable]
    public class LootTable
    {
        [Serializable]
        public class Entry
        {
            [field: SerializeField]
            public string ItemID { get; set; }

            [field: SerializeField]
            public int AmtMin { get; set; }
            [field: SerializeField]
            public int AmtMax { get; set; }

            [field: SerializeField]
            public float DropChance { get; set; }
        }

        [field: SerializeField]
        public Entry[] Entries { get; set; }

        public List<(Item item, int amt)> Generate()
        {
            System.Random rand = new System.Random();

            List<(Item item, int amt)> result = new List<(Item item, int amt)>();

            foreach( Entry entry in Entries )
            {
                if( entry.AmtMax < entry.AmtMin )
                {
                    throw new System.Exception( "Amt max less than min" );
                }

                int amt;
                if( entry.AmtMin == entry.AmtMax )
                {
                    amt = entry.AmtMin;
                }
                else
                {
                    amt = rand.Next( entry.AmtMin, entry.AmtMax );
                }

                result.Add( (GameRegistry.GetItem(entry.ItemID), amt) );
            }
            return result;
        }
    }

}
