using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TPS
{
    public class Character : MonoBehaviour
    {
        public float hp = 100f;
        public float moveSpeed = 5f;

        private float mouseX;

        private void Update()
        {
            Vector2 direction = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                direction.y += 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                direction.y += -1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                direction.x += -1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                direction.x += 1;
            }

            // Character Movement
            Vector3 moveDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0)
                * new Vector3(direction.x * moveSpeed * Time.deltaTime, 0, direction.y * moveSpeed * Time.deltaTime);

            transform.Translate(moveDirection, Space.World);
            
            // Camera Rotation
            float differ = mouseX - Input.mousePosition.x;
            mouseX = Input.mousePosition.x;

            if (differ > 0 || differ < 0)
            {
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - differ, 0);
            }
        }


        public void OnCollisionEnter(Collision collision)
        {
            #region Collision Event Example
            //Debug.Log("Character Collision Enter : " + collision.collider.name);

            //if (collision.collider.transform.TryGetComponent(out Trap trap))
            //{
            //    Debug.Log("Trap!!!!!!");
            //    // To do : HP Decrease Start ?
            //    // To do : HP Decrease Stop ?

            //    // 구현 목표 1: Trap 위에 올라가면 지속적으로 HP가 일정 속도로 깍임
            //    // 구현 목표 2. Trap에서 벗어나면 HP 깍이는 것을 멈춤
            //}
            //else
            //{
            //    Debug.Log("Not Collision Trap");
            //}
            #endregion
        }

        private void OnCollisionExit(Collision collision)
        {
            //Debug.Log("Character Collision Exit : " + collision.collider.name);
        }
    }
}