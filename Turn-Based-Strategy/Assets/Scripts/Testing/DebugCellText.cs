using Game.Grid;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugCellText : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    private object gridObject;
    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }
    protected virtual void Update()
    {
        text.text = gridObject.ToString();
    }
}
