using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPS
{
    public class EnemyCharacter : CharacterBase
    {
        public bool isChasing = false;

        private void Start()
        {
            PlayerCharacter playerCharacter = PlayerCharacter.Instance;
            playerCharacter.onMoving += OnPlayerMoved;
        }

        private void OnPlayerMoved(bool isPlayerMoving)
        {
            isChasing = isPlayerMoving;
            Debug.Log("Player Moved!! Let's Go Him!");
        }

        protected override void Update()
        {
            base.Update();

            if (isChasing)
            {
                PlayerCharacter playerCharacter = PlayerCharacter.Instance;
                if (playerCharacter != null)
                {
                    Vector3 playerPosition = playerCharacter.transform.position;
                    float distance = Vector3.Distance(playerPosition, transform.position);

                    if (distance > 3f)
                    {
                        transform.LookAt(playerPosition, Vector3.up);
                        transform.Translate(transform.forward * MoveSpeed * Time.deltaTime, Space.World);
                    }
                }
            }
        }
    }
}

