using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.AI;

namespace CSP
{
    public class CharacterBase : MonoBehaviour
    {
        public static List<CharacterBase> AllCharacters = new List<CharacterBase>();

        protected virtual void OnDrawGizmos()
        {
            if (characterCollider != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position + (Vector3.up * GroundCheckOffset),
                    characterCollider.radius);
            }
        }


        public bool IsSelected => isSelected;

        [field: Header("Unit Settings")]
        [field: SerializeField] public bool IsPlayerUnit { get; set; } = false;



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
        protected NavMeshAgent navAgent;

        protected bool isGrounded = false;
        protected int attackCombo = 0;

        protected GameObject selectionUI;
        protected bool isSelected = false;

        protected bool isRunning = false;
        protected CharacterBase targetCharacter;

        private Vector3 lastPosition = Vector3.zero;
        private bool isAgentMoved = false;

        // Callback Events - On Started Destination
        public System.Action OnDestinationStarted;
        public System.Action OnDestinationReached;

        protected virtual void Awake()
        {
            AllCharacters.Add(this);

            var selectionUIPrefab = Resources.Load<GameObject>("SelectionUI");
            selectionUI = Instantiate(selectionUIPrefab, transform);
            selectionUI.transform.localPosition = new Vector3(0, 0.1f, 0f);
            selectionUI.gameObject.SetActive(false);

            characterAnimator = GetComponentInChildren<Animator>();
            characterRigidbody = GetComponent<Rigidbody>();
            characterCollider = GetComponent<CapsuleCollider>();
            animationMessanger = GetComponentInChildren<AnimatorEventMessanger>();
            navAgent = GetComponent<NavMeshAgent>();
        }

        protected virtual void OnDestroy()
        {
            AllCharacters.Remove(this);
        }

        protected virtual void Update()
        {
            CheckGround();

            if (navAgent != null)
            {
                var velocity = navAgent.desiredVelocity;
                float magnitude = velocity.magnitude;
                bool isMoving = magnitude > 0;

                Vector3 differ = lastPosition - transform.position;

                characterAnimator.SetFloat("Speed", isRunning && isMoving ? 1f : isMoving ? 0.5f : 0f);
                characterAnimator.SetFloat("Horizontal", differ.normalized.x);
                characterAnimator.SetFloat("Vertical", differ.normalized.z);
            }

            CheckDestinationReached();
        }

        protected virtual void LateUpdate()
        {
            lastPosition = transform.position;
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


        public virtual void Attack() 
        {
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


        public void SelectCharacter(bool isSelected)
        {
            this.isSelected = isSelected;
            selectionUI.SetActive(isSelected);
        }

        public void SetDestination(Vector3 destination)
        {
            navAgent.SetDestination(destination);

            OnDestinationStarted?.Invoke();
            isAgentMoved = true;
        }

        public void CheckDestinationReached()
        {
            if (isAgentMoved)
            {
                if (navAgent.remainingDistance > 0f && navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    // Reached Destination
                    isAgentMoved = false;
                    OnDestinationReached?.Invoke();
                }
            }
        }

        public void ResetTarget()
        {
            targetCharacter = null;
        }
    }
}