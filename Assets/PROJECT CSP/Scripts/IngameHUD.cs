using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CSP
{
    public class IngameHUD : MonoBehaviour
    {
        public static IngameHUD Instance { get; private set; }

        public Image hpSlider;
        public Image spSlider;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void SetHealthPoint(float value)
        {
            hpSlider.fillAmount = value;
        }

        public void SetStaminaPoint(float value)
        {
            spSlider.fillAmount = value;
        }
    }
}

