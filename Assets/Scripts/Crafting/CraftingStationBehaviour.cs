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

        CraftingStationUI _uglyVariableUI; // this is ugly (separation of concerns: CraftingStationBehaviour shouldn't be aware that CraftingStationUI exists),
                                          // but works for now.
                                          // Holds the UI of this crafting station, so we can technically interact with multiple UIs at once.

        private void Start()
        {
            _interactible = this.GetComponent<InteractibleBehaviour>();

            _interactible.OnInteractionStart += ( r, e ) =>
            {
                if( r.GetComponent<PlayerController>() != null )
                {
                    _uglyVariableUI = CraftingStationUI.Create( GameManager.MainCanvas, this._craftingStation, (a) => Debug.Log( $"CRAFT {a} CLICKED" ) );
                    Debug.Log( "A PLAYER STARTED INTERACTING WITH ME!" );
                }
            };
            _interactible.OnInteractionEnd += ( r, e ) =>
            {
                if( r.GetComponent<PlayerController>() != null )
                {
                    Destroy( _uglyVariableUI.gameObject );
                    Debug.Log( "A PLAYER STOPPED INTERACTING WITH ME!" );
                }
            };
        }
    }
}