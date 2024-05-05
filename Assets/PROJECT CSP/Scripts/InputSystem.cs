using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSP
{
    public class InputSystem : MonoBehaviour
    {
        public bool IsCursorLocked = false;
        public RectTransform selectionBox;

        private Vector2 startClickPosition = Vector3.zero;

        private void Start()
        {
            selectionBox.sizeDelta = Vector2.zero;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startClickPosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 currentPosition = Input.mousePosition;
                float width = currentPosition.x - startClickPosition.x;
                float height = currentPosition.y - startClickPosition.y;

                selectionBox.anchoredPosition =
                    new Vector2(startClickPosition.x, startClickPosition.y) + new Vector2(width / 2, height / 2);

                selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            }

            if (Input.GetMouseButtonUp(0))
            {
                SelectCharacter();

                startClickPosition = Vector3.zero;
                selectionBox.sizeDelta = Vector2.zero;
            }

            if (Input.GetMouseButtonDown(1)) // Right Click
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 1000f, LayerMask.GetMask("Ground")))
                {
                    foreach (var character in CharacterBase.AllCharacters)
                    {
                        if (character.IsSelected)
                        {
                            character.SetDestination(hit.point);
                        }
                    }
                }
            }
        }

        private void SelectCharacter()
        {
            Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
            Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

            foreach (var character in CharacterBase.AllCharacters)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(character.transform.position);
                if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
                {
                    // Inside
                    character.SelectCharacter(true);
                }
                else
                {
                    // Outside
                    character.SelectCharacter(false);
                }
            }
        }

        private void CursorLock(bool isLock)
        {
            IsCursorLocked = isLock;
            //Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isLock;
        }
    }
}

