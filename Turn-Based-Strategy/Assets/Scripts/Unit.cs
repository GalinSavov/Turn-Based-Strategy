using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Units
{
    public class Unit : MonoBehaviour
    {
        private Vector3 targetPosition;
        private float speed = 2f;
        private float stoppingDistance = 0.1f;

        void Update()
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            if(Vector3.Distance(targetPosition, transform.position) > stoppingDistance)
            transform.position += moveDirection * speed * Time.deltaTime;

            if (Mouse.current.leftButton.isPressed)
                Move();
        }

        private void Move()
        {
           targetPosition = MouseWorldPosition.GetPosition();
        }

    }
}
