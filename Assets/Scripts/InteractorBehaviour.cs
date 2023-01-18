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
        public void InteractWith( InteractibleBehaviour interactee )
        {
            interactee.OnInteractWith?.Invoke( this, interactee );
        }
    }
}