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
        /// Defines interactions between this, and an other object.
        /// </summary>
        public Action<InteractorBehaviour, InteractibleBehaviour> OnInteractionStart;
        public Action<InteractorBehaviour, InteractibleBehaviour> OnInteractionEnd;

        public bool IsInteractingWith( InteractorBehaviour interactor )
        {
            return interactor.IsInteractingWith( this );
        }

        public void InteractWith( InteractorBehaviour interactor )
        {
            interactor.InteractWith( this );
        }

        public void StopInteracting( InteractorBehaviour interactor )
        {
            interactor.StopInteracting( this );
        }
    }
}