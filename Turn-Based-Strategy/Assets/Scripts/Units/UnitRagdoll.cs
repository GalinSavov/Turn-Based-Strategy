using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] Transform unitRagdollRootBone = null;

    public void Setup(Transform originalUnitRootBone)
    {
        MatchAllChildTransforms(originalUnitRootBone, unitRagdollRootBone);
    }
    //copies the values from the original character mesh to the ragdoll one
    private void MatchAllChildTransforms(Transform original, Transform clone)
    {
        foreach (Transform child in original)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MatchAllChildTransforms(child,cloneChild);
            }
        }
    }
}
