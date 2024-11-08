using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private GameObject unitRagdollPrefab = null;
    [SerializeField] private Transform originalUnitRootBone = null;
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
        health.OnDead += HandleOnDead;
    }

    private void HandleOnDead()
    {
        GameObject ragdoll = Instantiate(unitRagdollPrefab,transform.position,transform.rotation);
        UnitRagdoll unitRagdoll = ragdoll.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalUnitRootBone);
    }
}
