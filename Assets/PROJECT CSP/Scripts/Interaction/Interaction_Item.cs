using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class Interaction_Item : MonoBehaviour, IInteractable
    {
        public string InteractionKey => ITEM_ID;
        public string InteractionMessage => "Pick up";


        public string ITEM_ID;

        public void Interact()
        {
            Destroy(gameObject);

            InteractionUI.Instance.RemoveInteraction(InteractionKey);

            // To do : Add item to inventory
        }
    }
}
