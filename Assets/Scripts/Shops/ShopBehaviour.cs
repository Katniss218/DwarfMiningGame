using DwarfMiningGame.Inventories;
using DwarfMiningGame.Player;
using DwarfMiningGame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DwarfMiningGame.Shops
{
    [RequireComponent( typeof( InteractibleBehaviour ) )]
    public class ShopBehaviour : MonoBehaviour
    {
        // Buy shop is a shop that specifies certain items that can be bought from it by the player.

        // Interface looks like a list of items with their associated buy prices.


        [field: SerializeField]
        public Shop Shop { get; private set; }
        public InteractibleBehaviour Interactible { get; private set; }

        Dictionary<Shop.Offer, int> _remaining = null;

        ShopUI _uglyVarUI;                  // this is ugly (separation of concerns: CraftingStationBehaviour shouldn't be aware that CraftingStationUI exists),
                                            // but works for now.
                                            // Holds the UI of this crafting station, so we can interact with multiple crafting stations at once without them messing up each others UIs.
        PlayerInventory _uglyVarInventory; // this here because we need to unsubscribe from it when interaction ends.

        private void OnAfterMoneyChanged( float total )
        {
            _uglyVarUI.Redraw();
        }

        public void Restock()
        {
            this._remaining = new Dictionary<Shop.Offer, int>();
            foreach( var offer in this.Shop.BuyOffers )
            {
                if( offer.HasLimit )
                {
                    _remaining.Add( offer, offer.MaxRemaining );
                }
            }
        }

        public void SetShop( Shop shop )
        {
            this.Shop = shop;
            this.Restock();
        }

        /// <summary>
        /// Returns how many *ITEMS* (not offers) were bought.
        /// </summary>
        public int TryBuy( Shop.Offer offer, int amount = 1 )
        {
            if( !Shop.BuyOffers.Contains( offer ) )
            {
                return 0;
            }

            // Case with limit.
            if( _remaining.TryGetValue( offer, out int remaining ) )
            {
                if( remaining < 0 )
                {
                    return amount * offer.Amount;
                }
                if( remaining > amount )
                {
                    _remaining[offer] -= amount;
                    return amount * offer.Amount;
                }
                _remaining[offer] = 0;
                return remaining * offer.Amount;
            }

            // Case without limit.
            return amount * offer.Amount;
        }

        void Awake()
        {
            this.Interactible = this.GetComponent<InteractibleBehaviour>();

            // Restock if it wasn't stocked in prefab/scene.
            if( this.Shop != null && _remaining == null )
            {
                Restock();
            }
        }

        void Start()
        {
            Interactible.OnInteractionStart += OnInteractionStart;
            Interactible.OnInteractionStop += OnInteractionStop;
        }

        private void TryBuyItem( (Shop.Offer offer, int amount) e )
        {
            // We need to know in advance how many can be put in the inventory.
            // There is no way to do in cleanly because of the fact that the shop will only sell in multiples of `Offer.Amount`.
            int maxPossibleItemsSpace = _uglyVarInventory.GetSpaceLeft( e.offer.Item );
            int maxPossibleItemsMoney = Mathf.FloorToInt( _uglyVarInventory.Money / e.offer.Item.Value);

            int itemsToBuy = Math.Min( e.amount, maxPossibleItemsSpace );
            itemsToBuy = Math.Min( itemsToBuy, maxPossibleItemsMoney );

            // Now that we know, how many the inventory can buy, use that as a limit to get how many the shop will let you buy.
            // That way, we don't overflow the inventory.
            int itemsBought = this.TryBuy( e.offer, itemsToBuy );

            // And add the bought items to the inventory.
            int itemsBoughtInventory = _uglyVarInventory.TryBuyItem( e.offer.Item, itemsBought );
            if( itemsBought != itemsBoughtInventory )
            {
                Debug.LogWarning( $"itemsBought {itemsBought} != itemsBoughtInventory {itemsBoughtInventory}" );
            }
        }

        private void OnInteractionStart( InteractorBehaviour r, InteractibleBehaviour e )
        {
            PlayerBehaviour player = r.GetComponent<PlayerBehaviour>();
            if( player != null )
            {
                Debug.Log( $"Player '{player.gameObject.name}' interacted with '{this.gameObject.name}'." );

                _uglyVarInventory = player.Inventory;
                _uglyVarUI = ShopUI.Create( GameManager.MainCanvas, this, TryBuyItem );

                // Update the UI when the inventory changes, to reflect the current state of the inventory.
                _uglyVarInventory.OnAfterMoneyChanged += OnAfterMoneyChanged;
                _uglyVarUI.Redraw(); // Initialize
            }
        }

        private void OnInteractionStop( InteractorBehaviour r, InteractibleBehaviour e )
        {
            PlayerBehaviour player = r.GetComponent<PlayerBehaviour>();
            if( player != null )
            {
                Debug.Log( $"Player '{player.gameObject.name}' stopped interacting with '{this.gameObject.name}'." );

                Destroy( _uglyVarUI.gameObject );

                _uglyVarInventory.OnAfterMoneyChanged -= OnAfterMoneyChanged;
            }
        }
    }
}