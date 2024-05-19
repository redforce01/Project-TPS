using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class InteractionUI_Item : MonoBehaviour
    {
        public bool IsSelected
        {
            set => selection.SetActive(value);
        }

        public string InteractionMessage 
        { 
            set 
            { 
                text.text = value;
            } 
        }

        public string DataKey
        {
            get
            {
                return dataKey;
            }
            set
            {
                dataKey = value;
            }
        }

        public IInteractable InteractionData
        {
            get
            {
                return interactionData;
            }
            set
            {
                interactionData = value;
            }
        }


        public TMPro.TextMeshProUGUI text;
        public GameObject selection;

        private string dataKey;
        private IInteractable interactionData;

        public void Interact()
        {
            interactionData.Interact();
        }
    }
}
