using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class InputSystem : MonoBehaviour
    {
        public bool IsCursorLocked = false;

        private void Start()
        {
            CursorLock(true);
        }

        private void CursorLock(bool isLock)
        {
            IsCursorLocked = isLock;

            //Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isLock;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (false == IsCursorLocked)
                {
                    CursorLock(true);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CursorLock(false);
            }
        }
    }
}

