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

        public void SetAction(BaseAction action)
        {
            actionText.text = action.GetActionName().ToUpper();
        }
    }
}
