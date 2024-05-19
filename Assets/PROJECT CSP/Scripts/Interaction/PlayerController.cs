using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class PlayerController : MonoBehaviour
    {
        InteractionSensor interactionSensor;

        private void Awake()
        {
            interactionSensor = GetComponentInChildren<InteractionSensor>();

            interactionSensor.OnAddInteractable += OnDetectInteractable;
            interactionSensor.OnRemoveInteractable += OnLostInteractable;
        }

        void OnDetectInteractable(IInteractable interaction)
        {
            InteractionUI.Instance.AddInteraction(interaction.InteractionKey, interaction);
        }

        void OnLostInteractable(IInteractable interaction)
        {
            InteractionUI.Instance.RemoveInteraction(interaction.InteractionKey);
        }
    }
}
