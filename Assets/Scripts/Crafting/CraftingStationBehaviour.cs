using DwarfMiningGame.Player;
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
#warning TODO - Tiles are not crafting stations, since tiles can't have components (do we want it that way?).

        [SerializeField] private CraftingStation _craftingStation;

        InteractibleBehaviour _interactible;

        private void Start()
        {
            _interactible = this.GetComponent<InteractibleBehaviour>();

            _interactible.OnInteractWith += ( r, e ) =>
            {
                if( r.GetComponent<PlayerController>() != null )
                {
                    Debug.Log( "A PLAYER INTERACTED WITH ME!" );
                }    
            };
        }
    }
}