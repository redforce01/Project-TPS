using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class AnimatorEventMessanger : MonoBehaviour
    {
        public System.Action OnTriggeredExecuteDamage;

        public void ExecuteDamage()
        {
            OnTriggeredExecuteDamage?.Invoke();
        }
    }
}

