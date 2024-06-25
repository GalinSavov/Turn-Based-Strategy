using Game.Core;
using Game.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    private GridSystem gridSystem;
    void Start()
    {
        gridSystem = new GridSystem(10, 10,2f);
        Debug.Log(new GridPosition(5, 9));
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gridSystem.GetGridPositionFromWorld(MouseWorldPosition.GetPosition()));
    }
}
