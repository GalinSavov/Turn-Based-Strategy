using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathfindDebugCellText : DebugCellText
{
    [SerializeField] private TextMeshPro gCost = null;
    [SerializeField] private TextMeshPro hCost = null;
    [SerializeField] private TextMeshPro fCost = null;
    private PathNode pathNode;
    void Start()
    {
        UpdateNodeText();
    }
    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;
    }
    protected override void Update()
    {
        base.Update();
        UpdateNodeText();
    }
    private void UpdateNodeText()
    {
        gCost.text = pathNode.GCost.ToString();
        hCost.text = pathNode.HCost.ToString();
        fCost.text = pathNode.FCost.ToString();
    }
}
