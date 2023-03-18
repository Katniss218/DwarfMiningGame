using DwarfMiningGame.Inventories;
using DwarfMiningGame.Player;
using DwarfMiningGame.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame.Shops
{
    [RequireComponent( typeof( InteractibleBehaviour ) )]
    /// <summary>
    /// Add this to an object to turn it into a sell shop.
    /// </summary>
    public class SellShopBehaviour : MonoBehaviour
    {
        // Sell shop - opens a menu with all your items that the shop can buy from you.
        //           - Sell item after clicking.

        // Buy shop - opens a menu with shop's items. Buy item after clicking.

        public InteractibleBehaviour Interactible { get; private set; }

        SellShopUI _uglyVarUI;              // this is ugly (separation of concerns: CraftingStationBehaviour shouldn't be aware that CraftingStationUI exists),
                                            // but works for now.
                                            // Holds the UI of this crafting station, so we can interact with multiple crafting stations at once without them messing up each others UIs.
        PlayerInventory _uglyVarInventory; // this here because we need to unsubscribe from it when interaction ends.

        private void OnInventoryModified( (Item item, int amt) e )
        {
            _uglyVarUI.Redraw();
        }

        void Awake()
        {
            Interactible = this.GetComponent<InteractibleBehaviour>();
        }

        void Start()
        {
            Interactible.OnInteractionStart += OnInteractionStart;
            Interactible.OnInteractionStop += OnInteractionStop;
        }

        private void OnInteractionStart( InteractorBehaviour r, InteractibleBehaviour e )
        {
            PlayerBehaviour player = r.GetComponent<PlayerBehaviour>();
            if( player != null )
            {
                Debug.Log( $"Player '{player.gameObject.name}' interacted with '{this.gameObject.name}'." );

                _uglyVarInventory = player.Inventory;
                _uglyVarUI = SellShopUI.Create( GameManager.MainCanvas, this, () => _uglyVarInventory.GetItems(), ( e ) => _uglyVarInventory.TrySellItem( e.item, e.amount ) );

                // Update the UI when the inventory changes, to reflect the current state of the inventory.
                _uglyVarInventory.OnAdd += OnInventoryModified;
                _uglyVarInventory.OnRemove += OnInventoryModified;
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

                _uglyVarInventory.OnAdd -= OnInventoryModified;
                _uglyVarInventory.OnRemove -= OnInventoryModified;
            }
        }
    }
}