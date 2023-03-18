using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame
{
    /// <summary>
    /// An object that can perform interactions, can be interacted with.
    /// </summary>
    [DisallowMultipleComponent]
    public class InteractibleBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Defines what happens when this interactible starts interacting with a specific interactor.
        /// </summary>
        public Action<InteractorBehaviour, InteractibleBehaviour> OnInteractionStart;

        /// <summary>
        /// Defines what happens when this interactible stops interacting with a specific interactor.
        /// </summary>
        public Action<InteractorBehaviour, InteractibleBehaviour> OnInteractionStop;

        /// <summary>
        /// Checks if this interactible is interacting with a specific interactor.
        /// </summary>
        public bool IsInteractingWith( InteractorBehaviour interactor )
        {
            return interactor.IsInteractingWith( this );
        }

        /// <summary>
        /// Starts interacting with the specific interactor.
        /// </summary>
        public void InteractWith( InteractorBehaviour interactor )
        {
            interactor.InteractWith( this );
        }

        /// <summary>
        /// Shops interacting with the specific interactor.
        /// </summary>
        public void StopInteracting( InteractorBehaviour interactor )
        {
            interactor.StopInteracting( this );
        }
    }
}