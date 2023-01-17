using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfMiningGame
{
    public class Interactible : MonoBehaviour
    {
        public Action<Transform> OnInteract;

        public void Interact( Transform interactor )
        {
            OnInteract?.Invoke( interactor );
        }
    }
}