using DwarfMiningGame.Inventories;
using DwarfMiningGame.Player;
using DwarfMiningGame.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.Crafting
{
    [RequireComponent( typeof( InteractibleBehaviour ) )]
    /// <summary>
    /// Add this to an object to turn it into a crafting station.
    /// </summary>
    public class CraftingStationBehaviour : MonoBehaviour
    {
        [field: SerializeField] public CraftingStation CraftingStation { get; private set; }

        public InteractibleBehaviour Interactible { get; private set; }

        CraftingStationUI _uglyVarUI;       // this is ugly (separation of concerns: CraftingStationBehaviour shouldn't be aware that CraftingStationUI exists),
                                            // but works for now.
                                            // Holds the UI of this crafting station, so we can interact with multiple crafting stations at once without them messing up each others UIs.
        PlayerInventory _uglyVarInventory; // this here because we need to unsubscribe from it when interaction ends.

#warning TODO - The listeners should probably exist on the UI component and not on this object.
        // Currently the UI is the "dumb" object, and this is what manages it.

        private void OnInventoryModified( (Item item, int amt) e )
        {
            UpdateVisibleRecipes();
        }

        private void UpdateVisibleRecipes()
        {
            foreach( var recipe in this.CraftingStation.Recipes )
            {
                _uglyVarUI.SetEnabledRecipe( recipe, _uglyVarInventory.HasEnoughItems( recipe ) );
            }
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
                _uglyVarUI = CraftingStationUI.Create( GameManager.MainCanvas, this, ( a ) => _uglyVarInventory.TryCraft( a ) );

                // Update the UI when the inventory changes, to reflect the current state of the inventory.
                _uglyVarInventory.OnAdd += OnInventoryModified;
                _uglyVarInventory.OnRemove += OnInventoryModified;
                UpdateVisibleRecipes(); // Initialize.
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