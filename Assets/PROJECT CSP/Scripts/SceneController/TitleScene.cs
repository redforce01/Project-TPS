using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CSP
{
    public class TitleScene : SceneBase
    {
        public override IEnumerator OnStart()
        {
            LoadingProgress = 0f;
            var async = SceneManager.LoadSceneAsync("Title");
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
            //var async = SceneManager.UnloadSceneAsync("Title");
            //while (!async.isDone)
            //{
            //    LoadingProgress = async.progress / 0.9f;
            //    yield return null;
            //}
            yield return null;

            UIManager.Instance.Hide(UIList.TitleUI);
            LoadingProgress = 1f;
        }
    }
}
