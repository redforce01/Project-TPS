using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CSP
{
    public class Main : MonoBehaviour
    {
        public static Main Instance { get; private set; } = null;


        private SceneBase currentScene = null;
        private string currentSceneName = string.Empty;

        private void Awake()
        {
            Instance = this;

            GameObject uiManagerGo = new GameObject(typeof(CSP.UIManager).Name);
            uiManagerGo.transform.SetParent(transform);
            CSP.UIManager uiManager = uiManagerGo.AddComponent<CSP.UIManager>();
            uiManager.Initialize();

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            ChangeScene<TitleScene>();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void ChangeScene<T>() where T : SceneBase
        {
            if (string.IsNullOrEmpty(currentSceneName))
            {
                if (currentSceneName.Equals(typeof(T).Name))
                    return;
            }

            StartCoroutine(InternalChangeScene<T>());
        }

        IEnumerator InternalChangeScene<T>() where T : SceneBase
        {
            currentSceneName = typeof(T).Name;
            if (currentScene != null)
            {
                StartCoroutine(currentScene.OnEnd());
                yield return new WaitUntil(() => currentScene.LoadingProgress >= 1f);
                Destroy(currentScene.gameObject);
                currentScene = null;
            }

            GameObject sceneController = new GameObject(typeof(T).Name);
            sceneController.transform.SetParent(transform);
            currentScene = sceneController.AddComponent<T>();

            StartCoroutine(currentScene.OnStart());
            yield return new WaitUntil(() => currentScene.LoadingProgress >= 1f);
        }

        public void SystemQuit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            
            // To do : Save Data ? When Application Quit


        }
    }
}
