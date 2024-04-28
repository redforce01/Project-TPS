using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class LocomotionStateMachine : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var character =  animator.transform.root.GetComponent<CharacterBase>();
            character.AttackCombo = 0;
        }
    }
}