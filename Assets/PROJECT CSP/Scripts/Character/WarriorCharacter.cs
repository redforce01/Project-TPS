using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class WarriorCharacter : CharacterBase
    {
        public float detectionRange;
        public SensorBase sensor;

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
            sensor.OnDetected += OnDetected;
            sensor.OnLostDetection += OnLostDetection;
        }

        void OnDetected(Collider other)
        {

        }

        void OnLostDetection(Collider other)
        {

        }
    }
}

