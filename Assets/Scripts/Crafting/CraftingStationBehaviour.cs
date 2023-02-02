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
        [SerializeField] private CraftingStation _craftingStation;

        InteractibleBehaviour _interactible;

        CraftingStationUI _uglyVarUI;       // this is ugly (separation of concerns: CraftingStationBehaviour shouldn't be aware that CraftingStationUI exists),
                                            // but works for now.
                                            // Holds the UI of this crafting station, so we can technically interact with multiple UIs at once.
        PlayerInventory _uglyVarInventory;

        private void OnInventoryModified( (Item item, int amt) e )
        {
            foreach( var recipe in this._craftingStation.Recipes )
            {
                _uglyVarUI.SetEnabledRecipe( recipe, _uglyVarInventory.HasEnoughItems( recipe ) );
            }
        }

        private void Awake()
        {
            _interactible = this.GetComponent<InteractibleBehaviour>();
        }

        private void Start()
        {
            _interactible.OnInteractionStart += ( r, e ) =>
            {
                PlayerController player = r.GetComponent<PlayerController>();
                if( player != null )
                {
                    _uglyVarInventory = player.Inventory;
                    _uglyVarUI = CraftingStationUI.Create( GameManager.MainCanvas, this._craftingStation, ( a ) => _uglyVarInventory.Craft( a ) );

                    Debug.Log( "A PLAYER STARTED INTERACTING WITH ME!" );

                    _uglyVarInventory.OnAdd += OnInventoryModified;
                    _uglyVarInventory.OnRemove += OnInventoryModified;
                    OnInventoryModified( default );
                }
            };
            _interactible.OnInteractionEnd += ( r, e ) =>
            {
                if( r.GetComponent<PlayerController>() != null )
                {
                    Destroy( _uglyVarUI.gameObject );
                    Debug.Log( "A PLAYER STOPPED INTERACTING WITH ME!" );

                    _uglyVarInventory.OnAdd -= OnInventoryModified;
                    _uglyVarInventory.OnRemove -= OnInventoryModified;
                }
            };
        }
    }
}