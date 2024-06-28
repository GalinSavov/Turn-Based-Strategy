using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Grid
{
    public class GridSystemVisualSingle : MonoBehaviour
    {
        [SerializeField] private MeshRenderer MeshRenderer= null;

        public void Show()
        {
            MeshRenderer.enabled = true;
        }
        public void Hide()
        {
            MeshRenderer.enabled = false;
        }
    }

}