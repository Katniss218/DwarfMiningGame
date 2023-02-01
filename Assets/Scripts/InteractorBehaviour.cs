using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame
{
    /// <summary>
    /// An object that can initiate interactions.
    /// </summary>
    [DisallowMultipleComponent]
    public class InteractorBehaviour : MonoBehaviour
    {
        private List<InteractibleBehaviour> _currentInteractions = new List<InteractibleBehaviour>();

        [field: SerializeField]
        public float InteractionCooldown { get; set; }

        float _lastInteractionTimestamp = 0.0f;

        public bool IsInteractingWith( InteractibleBehaviour interactee )
        {
            return _currentInteractions.Contains( interactee );
        }

        public InteractibleBehaviour[] GetAllInteractions()
        {
            return _currentInteractions.ToArray();
        }

        public void InteractWith( InteractibleBehaviour interactee )
        {
            if( IsInteractingWith( interactee ) )
            {
                Debug.LogWarning( $"Tried to start another interaction with {interactee}. End the existing interaction first." );
                return;
            }

            if( Time.time > _lastInteractionTimestamp + InteractionCooldown )
            {
                interactee.OnInteractionStart?.Invoke( this, interactee );
                _currentInteractions.Add( interactee );
            }
        }

        public void StopInteracting( InteractibleBehaviour interactee )
        {
            if( !IsInteractingWith( interactee ) )
            {
                Debug.LogWarning( $"Tried to end a nonexistent interaction with {interactee}. Start an interaction first." );
                return;
            }

            interactee.OnInteractionEnd?.Invoke( this, interactee );
            _currentInteractions.Remove( interactee );
        }

        public void StopAllInteractions()
        {
            foreach( var interactee in GetAllInteractions() ) // important because interating with modification of the original array.
            {
                StopInteracting( interactee );
            }
        }
    }
}