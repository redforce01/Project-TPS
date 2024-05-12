using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class SensorBase : MonoBehaviour
    {
        public float DetectionRadius 
        { 
            get
            {
                return sensorCollider.radius;
            }
            set
            {
                sensorCollider.radius = value;
            }
        }

        public List<Collider> DetectedObjects => detectedObjects;


        public LayerMask detectionLayer;
        public string[] detectionTags;

        public System.Action<Collider> OnDetected;
        public System.Action<Collider> OnLostDetection;

        private List<Collider> detectedObjects = new List<Collider>();
        private SphereCollider sensorCollider;

        private void Awake()
        {
            sensorCollider = GetComponent<SphereCollider>();
            sensorCollider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (detectionLayer != (detectionLayer | (1 << other.gameObject.layer)))
            {
                return;
            }

            if (detectionTags.Length > 0)
            {
                bool isTagMatched = false;
                foreach (var tag in detectionTags)
                {
                    if (other.CompareTag(tag))
                    {
                        isTagMatched = true;
                        break;
                    }
                }

                if (!isTagMatched)
                {
                    return;
                }
            }

            // 예외처리 : 자기 자신이랑 충돌 된거면은 제외시키기 위해서 빠져나갔음
            if (other.transform == transform.root)
            {
                return;
            }

            detectedObjects.Add(other);
            OnDetected?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (detectedObjects.Exists(x => x == other))
            {
                detectedObjects.Remove(other);
                OnLostDetection?.Invoke(other);
            }
        }

        public void SetDetectionTags(string[] tags)
        {
            detectionTags = tags;
        }
    }
}

