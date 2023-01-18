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
        public Action<InteractorBehaviour, InteractibleBehaviour> OnInteractWith;

        [field: SerializeField]
        public float InteractionCooldown { get; set; }

        float _lastInteractionTimestamp = 0.0f;

        public void Interact( InteractorBehaviour interactor )
        {
            if( Time.time > _lastInteractionTimestamp + InteractionCooldown )
            {
                OnInteractWith?.Invoke( interactor, this );
            }
        }
    }
}