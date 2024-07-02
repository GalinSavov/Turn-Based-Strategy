using Game.Units;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UnitActionButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI actionText = null;
        [SerializeField] private Button actionButton = null;
        [SerializeField] private GameObject buttonImage = null;
        private BaseAction baseAction;

        public void SetAction(BaseAction action)
        {
            this.baseAction = action;

            actionText.text = action.GetActionName().ToUpper();
            actionButton.onClick.AddListener(() =>
            {
                UnitActionSystem.Instance.SetSelectedAction(action);

            });

        }
        public void UpdateSelectedVisual()
        {
            buttonImage.SetActive(baseAction == UnitActionSystem.Instance.GetSelectedAction());
        }
    }
}
