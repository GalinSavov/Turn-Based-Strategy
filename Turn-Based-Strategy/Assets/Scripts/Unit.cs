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


        private void Awake()
        {
            targetPosition = transform.position;
        }
        void Update()
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            
            if(Vector3.Distance(targetPosition, transform.position) > stoppingDistance)
            {
                unitAnimator.SetBool("isRunning", true);
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
                transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            }
            else
            unitAnimator.SetBool("isRunning",false);
   
        }

        public void Move()
        {
           targetPosition = MouseWorldPosition.GetPosition();
        }

    }
}
