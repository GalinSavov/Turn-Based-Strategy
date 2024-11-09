using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Transform objectToLookAtCamera = null;
    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    void LateUpdate()
    {
        objectToLookAtCamera.rotation = mainCamera.transform.rotation;
    }
}
