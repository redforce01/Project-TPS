using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPS
{
    public class CharacterBase : MonoBehaviour
    {
        public float HealthPoint = 100f;
        public float MoveSpeed = 5f;

        public float currentKnockbackForce = 0f;

        private void Update()
        {
            if (currentKnockbackForce > 0f)
            {
                transform.Translate(transform.forward * -1 * currentKnockbackForce * Time.deltaTime, Space.World);
                currentKnockbackForce -= Time.deltaTime;
            }
        }

        public void Damage(float damage, float knockbackForce)
        {
            HealthPoint -= damage;
            currentKnockbackForce = knockbackForce;

            if (HealthPoint <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}