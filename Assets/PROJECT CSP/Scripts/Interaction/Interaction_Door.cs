using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class Interaction_Door : MonoBehaviour, IInteractable
    {
        public string InteractionKey => "Door";
        public string InteractionMessage => "Open Door";


        private bool isOpen = false;

        public void Interact()
        {
            if (isOpen)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }

        private void Update()
        {
            if (isOpen)
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 90, 0), 0.1f);
            }
            else
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 180, 0), 0.1f);
            }
        }

        private void OpenDoor()
        {
            isOpen = true;
        }

        private void CloseDoor()
        {
            isOpen = false;
        }
    }
}
