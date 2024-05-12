using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class AttackStateMachine : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var character = animator.transform.root.GetComponent<WarriorCharacter>();
            character.IsAttackAnimProcess = true;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var character = animator.transform.root.GetComponent<WarriorCharacter>();
            character.IsAttackAnimProcess = false;
        }
    }
}