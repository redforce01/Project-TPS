using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CSP
{
    public class PlayerController : MonoBehaviour
    {
        /// <summary> 이 컴포넌트가 붙어있는 GameObject가 있으면 항상 작동함 </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var caremaTransform = Camera.main.transform;
            Gizmos.DrawLine(caremaTransform.position, caremaTransform.position + caremaTransform.forward * selectableRange);
        }

        /// <summary> 이 컴포넌트가 붙어있는 GameObject를 Hierachy 창에서 선택했을 때 작동함 </summary>
        private void OnDrawGizmosSelected()
        {
        }

        public Cinemachine.CinemachineVirtualCamera playerCamera;

        public float zoomDefault = 60f;
        public float zoomIn = 30f;

        public float selectableRange = 10f;

        private InteractionSensor interactionSensor;
        private MeshRenderer currentSelectableObjectRenderer;

        private void Awake()
        {
            interactionSensor = GetComponentInChildren<InteractionSensor>();

            interactionSensor.OnAddInteractable += OnDetectInteractable;
            interactionSensor.OnRemoveInteractable += OnLostInteractable;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                UIManager.Instance.Show(UIList.SimpleUI_A);
                UIManager.Instance.Hide(UIList.SimpleUI_B);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                UIManager.Instance.Hide(UIList.SimpleUI_A);
                UIManager.Instance.Show(UIList.SimpleUI_B);
            }


            bool isZooming = Input.GetKey(KeyCode.Mouse1); // Right Mouse Button
            playerCamera.m_Lens.FieldOfView = Mathf.Lerp(playerCamera.m_Lens.FieldOfView, 
                isZooming ? zoomIn : zoomDefault, Time.deltaTime * 5f);

            // transform.position에서 일정 범위(5f) 안에 있는 콜라이더들을 찾는 방법
            //Collider[] colliders = Physics.OverlapSphere(transform.position, 5f, LayerMask.GetMask("Character"));
            //var character = colliders[0].transform.root.GetComponent<Character>();
            //character.Damage(10f);

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // Screen Center Point
            if (Physics.Raycast(ray, out RaycastHit hit, selectableRange, LayerMask.GetMask("Selectable")))
            {
                if (currentSelectableObjectRenderer != null)
                {
                    currentSelectableObjectRenderer.material.color = Color.white;
                }

                currentSelectableObjectRenderer = hit.transform.GetComponent<MeshRenderer>();
                currentSelectableObjectRenderer.material.color = Color.red;
            }
            else
            {
                if (currentSelectableObjectRenderer != null)
                {
                    currentSelectableObjectRenderer.material.color = Color.white;
                    currentSelectableObjectRenderer = null;
                }
            }
        }

        void OnDetectInteractable(IInteractable interaction)
        {
            InteractionUI.Instance.AddInteraction(interaction.InteractionKey, interaction);
        }

        void OnLostInteractable(IInteractable interaction)
        {
            InteractionUI.Instance.RemoveInteraction(interaction.InteractionKey);
        }
    }
}
