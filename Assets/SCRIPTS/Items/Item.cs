using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Items
{
    [CreateAssetMenu( fileName = "item", menuName = "DwarfMiningGame/Item", order = 30 )]
    public class Item : ScriptableObject
    {
        [field: SerializeField]
        public string ID { get; set; }

        [field: SerializeField]
        public float Size { get; set; }

        [field: SerializeField]
        public Mesh Mesh { get; set; }

        [field: SerializeField]
        public Material Material { get; set; }

        public override bool Equals( object other )
        {
            if( other is Item item )
            {
                return this == item;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode() ^ Size.GetHashCode();
        }

        public static bool operator ==( Item i1, Item i2 )
        {
            return i1.ID == i2.ID;
        }

        public static bool operator !=( Item i1, Item i2 )
        {
            return i1.ID != i2.ID;
        }
    }
}