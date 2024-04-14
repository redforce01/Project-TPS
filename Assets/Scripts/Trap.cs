using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPS
{
    // Goal : Trap 에다가 버프 / 디버프 효과를 여러개 중복으로 줄 수 있는 시스템을 구축해보겠음

    [System.Flags]
    public enum TrapEffectType // enum 형은 기본형이 => Int32 타입이다.
    {
        None = 0,
        Damage      = 1 << 0,
        Heal        = 1 << 1,
        SpeedDown   = 1 << 2,
        SpeedUp     = 1 << 3,

        // All = int.MaxValue,
    }
         
    public class Trap : MonoBehaviour
    {
        private void OnDrawGizmos() // Unity Editor 에서만 동작하는 함수
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + offset, size);
        }

        public TrapEffectType effectType;
        public Vector3 size = new Vector3(1, 1, 1);
        public Vector3 offset = new Vector3(0, 0.5f, 0);

        public BoxCollider trapBox;


        private void Start()
        {
            // TrapSize Setup
            trapBox.size = size;
            trapBox.center = offset;
        }

    }
}

