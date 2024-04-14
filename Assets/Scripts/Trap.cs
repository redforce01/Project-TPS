using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPS
{
    // Goal : Trap ���ٰ� ���� / ����� ȿ���� ������ �ߺ����� �� �� �ִ� �ý����� �����غ�����

    [System.Flags]
    public enum TrapEffectType // enum ���� �⺻���� => Int32 Ÿ���̴�.
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
        private void OnDrawGizmos() // Unity Editor ������ �����ϴ� �Լ�
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

