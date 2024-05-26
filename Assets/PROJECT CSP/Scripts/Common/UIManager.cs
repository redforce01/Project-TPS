using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; } = null;

        public Dictionary<UIList, UIBase> uiContainer = new Dictionary<UIList, UIBase>();


        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void Initialize()
        {
            UIBase[] loadedUIs = Resources.LoadAll<UIBase>("UI/");
            loadedUIs.ForEach(ui => Debug.Log(ui.gameObject.name));

            foreach (UIBase ui in loadedUIs)
            {
                if (Enum.TryParse(ui.name, out UIList type))
                {
                    var newInstanceUI = Instantiate(ui, transform);
                    newInstanceUI.Hide();
                    uiContainer.Add(type, newInstanceUI);
                }
            }

            foreach (var ui in uiContainer)
            {
                Debug.Log($"UIType : {ui.Key}, UI Prefab Name : {ui.Value.gameObject.name}");
            }
        }

        public void Show(UIList type)
        {
            if (uiContainer.ContainsKey(type))
            {
                uiContainer[type].Show();
            }
        }

        public void Hide(UIList type)
        {
            if (uiContainer.ContainsKey(type))
            {
                uiContainer[type].Hide();
            }
        }
    }
}
