using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class TitleUI : UIBase
    {
        public void OnClickStartButton()
        {
            Main.Instance.ChangeScene<GameScene>();
        }

        public void OnClickSettingButton()
        {
            // To do : Show Setting UI

        }

        public void OnClickExitButton()
        {
            Main.Instance.SystemQuit();
        }
    }
}
