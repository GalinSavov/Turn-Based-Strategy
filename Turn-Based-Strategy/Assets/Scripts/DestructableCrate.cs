using Game.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableCrate : MonoBehaviour
{
    public static Action<GridPosition> onAnyCrateDestroyed;
    [SerializeField] private Transform crateDestroyedPrefab = null;
    public void Damage()
    {
        Transform crateDestroyed = Instantiate(crateDestroyedPrefab,transform.position, transform.rotation);
        ApplyForceToCrateParts(crateDestroyed, 150f, transform.position, 10f);
        Destroy(gameObject);
        onAnyCrateDestroyed?.Invoke(LevelGrid.Instance.GetGridPosition(transform.position));
    }
    private void ApplyForceToCrateParts(Transform root,float explosionForce,Vector3 explosionPosition,float explosionRange)
    {
        foreach (Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }
            ApplyForceToCrateParts(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
