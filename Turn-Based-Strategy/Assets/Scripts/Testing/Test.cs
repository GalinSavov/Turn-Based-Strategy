using Game.Core;
using Game.Grid;
using Game.Units;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] GridSystemVisual gridSystemVisual;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Mouse.current.leftButton.isPressed)
        {
            
        }
    }
}
