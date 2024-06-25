using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Core
{
    public class MouseWorldPosition : MonoBehaviour
    {
        [SerializeField] private LayerMask floorLayerMask;
        private static MouseWorldPosition Instance;

        private void Awake()
        {
            Instance = this;
        }
        
        public static Vector3 GetPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Instance.floorLayerMask)) 
                return hit.point;

            return Vector3.zero;
        }
    }
}
