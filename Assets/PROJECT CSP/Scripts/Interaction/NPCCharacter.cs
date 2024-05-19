using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class NPCCharacter : MonoBehaviour, IInteractable
    {
        public string InteractionMessage => NPC_Name;
        public string InteractionKey => NPC_ID.ToString();


        public int NPC_ID;
        public string NPC_Name;


        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Interact()
        {
            switch (NPC_ID)
            {
                case 1:
                    animator.SetTrigger("Interaction_1");
                    break;
                case 2:
                    animator.SetTrigger("Interaction_2");
                    break;
                case 3:
                    animator.SetTrigger("Interaction_3");
                    break;
            }
        }
    }
}
