using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPS
{
    public class CharacterBase : MonoBehaviour
    {
        public Animator characterAnimator;

        public float HealthPoint = 100f;
        public float StaminaPoint = 100f;
        public float MoveSpeed = 5f;
        public float currentKnockbackForce = 0f;

        /// <summary> Damaged 를 받았을 때 불려지는 Callback Event </summary>
        public System.Action<float, float> OnDamagedCallback;


        public delegate void OnMoving(bool isMoving);
        public OnMoving onMoving;

        protected virtual void Awake()
        {
            characterAnimator = GetComponentInChildren<Animator>();
        }

        protected virtual void Update()
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

            //if (OnDamaged != null)
            //{
            //    OnDamaged.Invoke(HealthPoint, damage);
            //}
            OnDamagedCallback?.Invoke(HealthPoint, damage);

            if (HealthPoint <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}