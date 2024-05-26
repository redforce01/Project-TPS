using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public abstract class SceneBase : MonoBehaviour
    {
        public float LoadingProgress { get; set; }

        public abstract IEnumerator OnStart();
        public abstract IEnumerator OnEnd();
    }
}
