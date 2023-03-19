using DwarfMiningGame.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Shops
{
    /// <summary>
    /// A definition of a shop.
    /// </summary>
    [CreateAssetMenu( fileName = "shop", menuName = "DwarfMiningGame/Shop (Buy)", order = 610 )]
    public class Shop : ScriptableObject, IIdentifiable
    {
        [Serializable]
        public class Offer
        {
            [field: SerializeField]
            public Item Item { get; set; }

            [field: SerializeField]
            public int Amount { get; set; } = 1;

            [field: SerializeField]
            public bool HasLimit { get; set; } = false;

            [field: SerializeField]
            public int MaxRemaining { get; set; } = 0;
        }

        [field: SerializeField]
        public string ID { get; set; } = "shop.invalid";

        [field: SerializeField]
        public Offer[] BuyOffers { get; set; }
    }
}