using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace CSP
{
    public enum ActionStateType
    {
        None = 0,

        Idle,
        Move,
        Combat,
    }


    public class WarriorCharacter : CharacterBase
    {
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (pivotPoint != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(pivotPoint.position, patrolRange);
            }
        }

        public bool IsAttackAnimProcess
        {
            get => isAttackAnimationProcessing;
            set => isAttackAnimationProcessing = value;
        }


        [Header("Warrior Settings")]
        public float detectionRange;
        public float attackRange;
        public float attackDamage;
        public SensorBase sensor;

        [Header("AI State Settings")]
        public Transform pivotPoint;
        public float patrolRange = 10f;
        public ActionStateType defaultState;

        private ActionStateType currentState = ActionStateType.None;
        private bool isAttackAnimationProcessing = false;

        public void SetState(ActionStateType newState)
        {
            if (currentState == newState)
            {
                return;
            }

            ExitState();
            currentState = newState;
            EnterState();
        }

        public void EnterState()
        {
            switch (currentState)
            {
                case ActionStateType.Idle:
                    break;
                case ActionStateType.Move:
                    break;
                case ActionStateType.Combat:
                    break;
            }
        }

        public void ExitState()
        {
            switch (currentState)
            {
                case ActionStateType.Idle:
                    break;
                case ActionStateType.Move:
                    break;
                case ActionStateType.Combat:
                    break;
            }
        }

        public void UpdateState()
        {
            switch (currentState)
            {
                case ActionStateType.Idle:
                    break;
                case ActionStateType.Move:
                    break;
                case ActionStateType.Combat:
                    {
                        if (targetCharacter != null)
                        {
                            float distance = Vector3.Distance(transform.position, targetCharacter.transform.position);
                            if (distance < attackRange)
                            {
                                // Attack
                                ExecuteAttack();
                            }
                            else
                            {
                                // Chase
                                base.SetDestination(targetCharacter.transform.position);
                            }
                        }
                    }
                    break;
            }
        }



        protected override void Awake()
        {
            base.Awake();

            var swordPrefab = Resources.Load<GameObject>("Sword");

            var rightHandTransform = characterAnimator.GetBoneTransform(HumanBodyBones.RightHand);
            var newSwordInstance = Instantiate(swordPrefab, rightHandTransform);
            newSwordInstance.transform.localScale = Vector3.one * 100;
            newSwordInstance.transform.localRotation = Quaternion.Euler(180, 0, 0);

            var sensorPrefab = Resources.Load<SensorBase>("CSP.CharacterSensor");
            sensor = Instantiate(sensorPrefab, transform);
            sensor.DetectionRadius = detectionRange;
            sensor.detectionLayer = LayerMask.GetMask("Character");
        }

        private void Start()
        {
            animationMessanger.OnTriggeredExecuteDamage += Attack;

            sensor.OnDetected += OnDetected;
            sensor.OnLostDetection += OnLostDetection;

            if (base.IsPlayerUnit)
            {
                transform.tag = "Team 1";
                sensor.SetDetectionTags(new string[] { "Team 2" });
            }
            else
            {
                transform.tag = "Team 2";
                sensor.SetDetectionTags(new string[] { "Team 1" });
            }

            base.OnDestinationStarted += OnStartMoveCallback;
            base.OnDestinationReached += OnDestinationReachedCallback;

            EnterState();
        }

        protected override void Update()
        {
            base.Update();

            UpdateState();
        }

        public override void Attack()
        {
            targetCharacter.Damage(attackDamage);
        }

        public void ExecuteAttack()
        {
            if (isAttackAnimationProcessing)
                return;

            characterAnimator.SetTrigger("Attack_Warrior");            
        }

        private void OnStartMoveCallback()
        {
            bool hasDetectedEnemy = sensor.DetectedObjects.Count > 0;
            if (hasDetectedEnemy)
            {
                SetState(ActionStateType.Combat);
            }
            else
            {
                SetState(ActionStateType.Move);
            }
        }

        private void OnDestinationReachedCallback()
        {
            bool hasDetectedEnemy = sensor.DetectedObjects.Count > 0;
            if (hasDetectedEnemy)
            {
                SetState(ActionStateType.Combat);
            }
            else
            {
                SetState(ActionStateType.Idle);
            }
        }

        private void OnDetected(Collider other)
        {
            if (targetCharacter == null)
            {
                if (other.transform.root.TryGetComponent(out CharacterBase enemy))
                {
                    targetCharacter = enemy;
                }
            }
        }

        private void OnLostDetection(Collider other)
        {

        }
    }
}

