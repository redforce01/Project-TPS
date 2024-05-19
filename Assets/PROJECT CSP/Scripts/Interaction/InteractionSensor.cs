using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class InteractionSensor : MonoBehaviour
    {
        public List<IInteractable> interactionList = new List<IInteractable>();

        public System.Action<IInteractable> OnAddInteractable;
        public System.Action<IInteractable> OnRemoveInteractable;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.TryGetComponent(out IInteractable interaction))
            {
                interactionList.Add(interaction);

                OnAddInteractable?.Invoke(interaction);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.TryGetComponent(out IInteractable interaction))
            {
                interactionList.Remove(interaction);

                OnRemoveInteractable?.Invoke(interaction);
            }
        }
    }
}
