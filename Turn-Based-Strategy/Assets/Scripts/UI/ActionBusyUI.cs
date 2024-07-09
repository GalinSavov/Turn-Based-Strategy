using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ActionBusyUI : MonoBehaviour
    {
        [SerializeField] private GameObject ui = null;

        private void OnEnable()
        {
            UnitActionSystem.Instance.OnCurrentlyInActionChanged += HandleOnCurrentlyInActionChanged;
        }
        private void OnDisable()
        {
            UnitActionSystem.Instance.OnCurrentlyInActionChanged -= HandleOnCurrentlyInActionChanged;
        }
        private void HandleOnCurrentlyInActionChanged(bool isBusy)
        {
            ui.SetActive(isBusy);
        }
    }
}
