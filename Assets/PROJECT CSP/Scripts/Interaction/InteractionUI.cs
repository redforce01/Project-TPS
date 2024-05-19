using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class InteractionUI : MonoBehaviour
    {
        public static InteractionUI Instance { get; private set; } = null;

        public List<InteractionUI_Item> interactionList = new List<InteractionUI_Item>();
        public InteractionUI_Item listItemPrefab;
        public Transform listItemRoot;

        private InteractionUI_Item selectedItem = null;
        private int selectedIndex = 0;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (selectedItem != null)
                {
                    selectedItem.Interact();
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (selectedItem != null)
                {
                    selectedItem.IsSelected = false;
                }

                if (interactionList.Count > 0)
                {
                    selectedIndex++;
                    selectedIndex = Mathf.Clamp(selectedIndex, 0, interactionList.Count - 1);

                    selectedItem = interactionList[selectedIndex];
                    selectedItem.IsSelected = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (selectedItem != null)
                {
                    selectedItem.IsSelected = false;
                }

                if (interactionList.Count > 0)
                {
                    selectedIndex--;
                    selectedIndex = Mathf.Clamp(selectedIndex, 0, interactionList.Count - 1);

                    selectedItem = interactionList[selectedIndex];
                    selectedItem.IsSelected = true;
                }
            }
        }

        public void AddInteraction(string key, IInteractable interaction)
        {
            var newListItem = Instantiate(listItemPrefab, listItemRoot);
            newListItem.gameObject.SetActive(true);

            newListItem.InteractionMessage = interaction.InteractionMessage;
            newListItem.DataKey = key;
            newListItem.InteractionData = interaction;

            interactionList.Add(newListItem);
        }

        public void RemoveInteraction(string key)
        {
            var target = interactionList.Find(x => x.DataKey.Equals(key));
            if (target != null)
            {
                interactionList.Remove(target);
                Destroy(target.gameObject);
            }
        }
    }
}
