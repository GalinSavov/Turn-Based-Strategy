using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Units
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private Animator unitAnimator = null;
        private Vector3 targetPosition;
        private float moveSpeed = 2f;
        private float rotateSpeed = 10f;
        private float stoppingDistance = 0.1f;

        void Update()
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            
            if(Vector3.Distance(targetPosition, transform.position) > stoppingDistance)
            {
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
                transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            }
            else
            unitAnimator.SetBool("isRunning",false);

            if (Mouse.current.leftButton.isPressed)
            {
                Move();
                unitAnimator.SetBool("isRunning", true);
            }

            Debug.Log("Move direction: " + moveDirection);
            Debug.Log("Target Position: " + targetPosition);
        }

        private void Move()
        {
           targetPosition = MouseWorldPosition.GetPosition();
        }

    }
}
