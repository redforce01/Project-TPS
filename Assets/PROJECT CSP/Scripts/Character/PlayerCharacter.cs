using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class PlayerCharacter : CharacterBase
    {
        public static PlayerCharacter Instance;

        [Header("Player Settings")]
        public GameObject bullet;
        public Transform bulletFireStartPosition;
        public float fireRate = 0.03f;
        public float bulletForce = 100f;

        private float mouseX;
        private TrapEffectType activatedEffects = TrapEffectType.None;
        private float fireTime = 0f;

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        protected override void Update()
        {
            base.Update();

            InputMovement();
            InputRotate();
            InputJump();

            if (Input.GetMouseButton(0)) // Left Mouse Button
            {
                #region Shooting Logic
                //// To do : Fire Bullet
                //if (Time.time - fireRate > fireTime) // Available Fire
                //{
                //    fireTime = Time.time;
                //    GameObject newBullet = Instantiate(bullet, bulletFireStartPosition.position, bulletFireStartPosition.rotation);
                //    var bulletRigid = newBullet.GetComponent<Rigidbody>();
                //    bulletRigid.AddForce(transform.forward * bulletForce, ForceMode.Impulse);
                //}
                #endregion
            }

            if (Input.GetMouseButtonDown(0)) // Left Mouse Button Down
            {
                base.DoLightAttack();
            }

            if (Input.GetMouseButtonDown(1)) // Right Mouse Button Down
            {
                base.DoHeavyAttack();
            }

            // Effect Check & Update
            if (activatedEffects.HasFlag(TrapEffectType.Damage))
            {
                Damage(1f * Time.deltaTime);
            }

            if (activatedEffects.HasFlag(TrapEffectType.Heal))
            {
                HealthPoint += 1f * Time.deltaTime;
            }
        }

        private void InputJump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                base.Jump();
            }
        }

        private void InputRotate()
        {
            // Camera Rotation
            float differ = mouseX - Input.mousePosition.x;
            mouseX = Input.mousePosition.x;

            if (differ > 0 || differ < 0)
            {
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - differ, 0);
            }
        }

        private void InputMovement()
        {
            Vector2 direction = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                direction.y += 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                direction.y += -1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                direction.x += -1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                direction.x += 1;
            }

            bool isInputRunning = Input.GetKey(KeyCode.LeftShift);
            base.Move(direction, isInputRunning);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent(out Trap trap))
            {
                var effectTypes = trap.effectType;

                if (effectTypes.HasFlag(TrapEffectType.Damage))
                {
                    activatedEffects |= TrapEffectType.Damage;
                }

                if (effectTypes.HasFlag(TrapEffectType.Heal))
                {
                    activatedEffects |= TrapEffectType.Heal;
                }

                if (effectTypes.HasFlag(TrapEffectType.SpeedUp))
                {
                    activatedEffects |= TrapEffectType.SpeedUp;

                    MoveSpeed = 10f;
                }

                if (effectTypes.HasFlag(TrapEffectType.SpeedDown))
                {
                    activatedEffects |= TrapEffectType.SpeedDown;

                    MoveSpeed = 2f;
                }

                RefreshEffect();
            }
        }

        private void RefreshEffect()
        {
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.transform.TryGetComponent(out Trap trap))
            {
                var effectTypes = trap.effectType;

                if (effectTypes.HasFlag(TrapEffectType.Damage))
                {
                    activatedEffects &= ~TrapEffectType.Damage;
                }

                if (effectTypes.HasFlag(TrapEffectType.Heal))
                {
                    activatedEffects &= ~TrapEffectType.Heal;
                }

                if (effectTypes.HasFlag(TrapEffectType.SpeedUp))
                {
                    activatedEffects &= ~TrapEffectType.SpeedUp;

                    MoveSpeed = 5f;
                }

                if (effectTypes.HasFlag(TrapEffectType.SpeedDown))
                {
                    activatedEffects &= ~TrapEffectType.SpeedDown;

                    MoveSpeed = 5f;
                }
            }
        }
    }
}