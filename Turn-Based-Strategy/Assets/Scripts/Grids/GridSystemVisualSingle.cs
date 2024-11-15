using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Grid
{
    public class GridSystemVisualSingle : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer = null;

        public void Show(Material gridMaterial)
        {
            meshRenderer.enabled = true;
            meshRenderer.material = gridMaterial;
        }
        public void Hide()
        {
            meshRenderer.enabled = false;
        }
    }

}