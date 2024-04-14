using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPS
{
    public class Bullet : MonoBehaviour
    {
        public float lifeTime = 3f;
        public float bulletSpeed = 100f;
        public float damage = 1f;
        public float knockbackForce = 1f;

        private float startTime;

        private void Start()
        {
            startTime = Time.time;
        }

        private void Update()
        {
            if (Time.time > startTime + lifeTime)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // 캐릭터한테 맞췄는가? 를 체크
            if (collision.transform.root.TryGetComponent(out CharacterBase character))
            {
                // To do : Character Blood Effect + Sound + Damage
                character.Damage(damage, knockbackForce);

                Destroy(gameObject);
            }
            else // 캐릭터가 아닌 다른 물체에 맞았을 때
            {
                // To do : Effect or Sound
            }
        }
    }
}

