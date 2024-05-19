using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public interface IInteractable
    {
        public string InteractionKey { get; }
        public string InteractionMessage { get; }

        public void Interact();
    }
}
