using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class CharacterBase : MonoBehaviour
    {
        public void OnDrawGizmos()
        {
            if (characterCollider != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position + (Vector3.up * GroundCheckOffset),
                    characterCollider.radius);
            }
        }

        [field: Header("CharacterBase Settings")]
        [field: SerializeField] public float HealthPoint { get; set; } = 100f;
        [field: SerializeField] public float StaminaPoint { get; set; } = 100f;
        [field: SerializeField] public float MoveSpeed { get; set; } = 5f;


        [field: SerializeField] public float GroundCheckOffset { get; set; } = 0.1f;
        [field: SerializeField] public float JumpForce { get; set; } = 10f;
        [field: SerializeField] public LayerMask GroundLayer { get; private set; }

        public int AttackCombo
        {
            get
            {
                return attackCombo;
            }
            set
            {
                attackCombo = value;
                characterAnimator.SetInteger("ComboCount", attackCombo);
            }
        }


        protected Animator characterAnimator;
        protected Rigidbody characterRigidbody;
        protected CapsuleCollider characterCollider;
        protected AnimatorEventMessanger animationMessanger;

        protected bool isGrounded = false;
        protected int attackCombo = 0;


        protected virtual void Awake()
        {
            characterAnimator = GetComponentInChildren<Animator>();
            characterRigidbody = GetComponent<Rigidbody>();
            characterCollider = GetComponent<CapsuleCollider>();
            animationMessanger = GetComponentInChildren<AnimatorEventMessanger>();
        }

        protected virtual void Update()
        {
            CheckGround();
        }

        private void CheckGround()
        {
            Vector3 position = transform.position + (Vector3.up * GroundCheckOffset);
            if (Physics.CheckSphere(position, characterCollider.radius, GroundLayer,
                QueryTriggerInteraction.Ignore))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }

            characterAnimator.SetBool("IsGrounded", isGrounded);
        }

        public void Damage(float damage)
        {
            HealthPoint -= damage;
            if (HealthPoint <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void Move(Vector2 direction, bool isRunning = true)
        {
            // Character Movement
            Vector3 moveDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0)
                * new Vector3(direction.x * MoveSpeed * Time.deltaTime, 0, direction.y * MoveSpeed * Time.deltaTime);

            transform.Translate(moveDirection, Space.World);

            float magnitude = direction.normalized.magnitude;
            bool isMoving = magnitude > 0;
            characterAnimator.SetFloat("Speed", isRunning && isMoving ? 1f : isMoving ? 0.5f : 0f);
            characterAnimator.SetFloat("Horizontal", direction.x);
            characterAnimator.SetFloat("Vertical", direction.y);
        }

        public void Jump()
        {
            if (!isGrounded)
                return;

            isGrounded = false;
            characterAnimator.SetTrigger("Jump");
            characterRigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Force);
        }

        public void DoLightAttack()
        {
            if (AttackCombo <= 0)
            {
                characterAnimator.SetTrigger("Attack_Light");
            }

            AttackCombo++;
            characterAnimator.SetInteger("ComboCount", AttackCombo);
        }

        public void DoHeavyAttack()
        {
            characterAnimator.SetTrigger("Attack_Heavy");
        }

        public void DamageExecute()
        {

        }
    }
}