using Game.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour,IInteractable
{
    private GridPosition gridPosition;
    [SerializeField] private bool isOpen = false;
    [SerializeField] private Animator doorAnimator = null;
    private void Start()
    {
        this.gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
        Pathfinding.Instance.GetNode(gridPosition.x, gridPosition.z).Walkable = false;
    }
    public void Interact()
    {
        if (isOpen)
            CloseDoor();
        else 
            OpenDoor();
    }
    private void OpenDoor()
    {
        isOpen = true;
        doorAnimator.SetBool("isOpen", isOpen);
        Pathfinding.Instance.GetNode(gridPosition.x, gridPosition.z).Walkable = true;
    }
    private void CloseDoor()
    {
        isOpen = false;
        doorAnimator.SetBool("isOpen", isOpen);
        Pathfinding.Instance.GetNode(gridPosition.x, gridPosition.z).Walkable = false;
    }
}
