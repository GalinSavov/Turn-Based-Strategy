using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Units
{
    public class UnitSelectedVisual : MonoBehaviour
    {
        [SerializeField] private Unit unit = null;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            UpdateSpriteVisual();
            UnitActionSystem.Instance.OnSelectedUnitChanged += HandleOnSelectedUnitChanged;
        }
        private void OnDisable()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged -= HandleOnSelectedUnitChanged;
        }

        private void HandleOnSelectedUnitChanged()
        {
            UpdateSpriteVisual();
        }
        private void UpdateSpriteVisual()
        {
            if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }


    }
}
