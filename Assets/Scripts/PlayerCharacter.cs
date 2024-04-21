using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPS
{
    public class PlayerCharacter : CharacterBase
    {
        public static PlayerCharacter Instance;

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

            base.OnDamagedCallback += OnDamaged;
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

            if (Input.GetMouseButton(0)) // Left Mouse Button
            {
                // To do : Fire Bullet
                if (Time.time - fireRate > fireTime) // Available Fire
                {
                    fireTime = Time.time;
                    GameObject newBullet = Instantiate(bullet, bulletFireStartPosition.position, bulletFireStartPosition.rotation);
                    var bulletRigid = newBullet.GetComponent<Rigidbody>();
                    bulletRigid.AddForce(transform.forward * bulletForce, ForceMode.Impulse);
                }
            }

            // Effect Check & Update
            if (activatedEffects.HasFlag(TrapEffectType.Damage))
            {
                Damage(1f * Time.deltaTime, 0f);
            }

            if (activatedEffects.HasFlag(TrapEffectType.Heal))
            {
                HealthPoint += 1f * Time.deltaTime;
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

            // Character Movement
            Vector3 moveDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0)
                * new Vector3(direction.x * MoveSpeed * Time.deltaTime, 0, direction.y * MoveSpeed * Time.deltaTime);

            transform.Translate(moveDirection, Space.World);

            float magnitude = direction.normalized.magnitude;
            bool isMoving = magnitude > 0;
            characterAnimator.SetFloat("Speed", isInputRunning && isMoving ? 1f : isMoving ? 0.5f : 0f);
            characterAnimator.SetFloat("Horizontal", direction.x);
            characterAnimator.SetFloat("Vertical", direction.y);

            base.onMoving(isMoving);
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


        public void OnCollisionEnter(Collision collision)
        {
            #region Collision Event Example
            //Debug.Log("Character Collision Enter : " + collision.collider.name);

            //if (collision.collider.transform.TryGetComponent(out Trap trap))
            //{
            //    Debug.Log("Trap!!!!!!");
            //    // To do : HP Decrease Start ?
            //    // To do : HP Decrease Stop ?

            //    // 구현 목표 1: Trap 위에 올라가면 지속적으로 HP가 일정 속도로 깍임
            //    // 구현 목표 2. Trap에서 벗어나면 HP 깍이는 것을 멈춤
            //}
            //else
            //{
            //    Debug.Log("Not Collision Trap");
            //}
            #endregion
        }

        private void OnCollisionExit(Collision collision)
        {
            //Debug.Log("Character Collision Exit : " + collision.collider.name);
        }

        private void OnDamaged(float remainHealthPoint, float damage)
        {
            Debug.Log("Player Character Take Damage!! : " + damage + "Remain HP : " + remainHealthPoint);

            IngameHUD.Instance.SetHealthPoint(remainHealthPoint / 100f);
        }
    }
}