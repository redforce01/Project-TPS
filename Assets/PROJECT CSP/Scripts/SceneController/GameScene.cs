using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CSP
{
    public class GameScene : SceneBase
    {
        public override IEnumerator OnStart()
        {
            LoadingProgress = 0f;
            var async = SceneManager.LoadSceneAsync("GameScene");
            while (!async.isDone)
            {
                LoadingProgress = async.progress / 0.9f;
                yield return null;
            }

            LoadingProgress = 1f;
            UIManager.Instance.Show(UIList.TitleUI);
        }

        public override IEnumerator OnEnd()
        {
            LoadingProgress = 0f;
            var async = SceneManager.UnloadSceneAsync("GameScene");
            while (!async.isDone)
            {
                LoadingProgress = async.progress / 0.9f;
                yield return null;
            }

            LoadingProgress = 1f;
        }
    }
}
