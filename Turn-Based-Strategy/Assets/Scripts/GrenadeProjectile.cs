using Game.Grid;
using Game.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    private Vector3 targetWorldPosition;
    private float totalDistance;
    private Vector3 positionXZ;
    private Action onGrenadeBehaviourComplete;
    public static Action onAnyGrenadeExploded;
    [SerializeField] private float grenadeSpeed = 5f;
    [SerializeField] private ParticleSystem grenadeExplosion = null;
    [SerializeField] private TrailRenderer trailRenderer = null;
    [SerializeField] private AnimationCurve animationCurve = null;
    public void Setup(GridPosition targetGridPosition,Action onGrenadeBehaviourComplete)
    {
        targetWorldPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetWorldPosition); //total distance between a and b
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
    }
    private void Update()
    {
        Vector3 grenadeDir = (targetWorldPosition - positionXZ).normalized;
        positionXZ += grenadeDir * Time.deltaTime * grenadeSpeed;
        float distance = Vector3.Distance(positionXZ,targetWorldPosition); //current distance every frame between a and b
        float distanceNormalized = 1 - distance / totalDistance;
        float maxHeight = totalDistance / 3f; // balance out the parabola, the further the distance - the bigger the height
        float positionY = animationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x,positionY,positionXZ.z);

        if (Vector3.Distance(positionXZ,targetWorldPosition) < 0.1f)
        {
            float damageRadius = 2f;
            Collider[] colliders = Physics.OverlapSphere(targetWorldPosition, damageRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.TakeDamage(25);
                }
                if (collider.TryGetComponent<DestructableCrate>(out DestructableCrate crate))
                {
                    crate.Damage();
                }
            }
            Destroy(gameObject);
            trailRenderer.transform.parent = null;
            Instantiate(grenadeExplosion, targetWorldPosition + Vector3.up * 1.25f, Quaternion.identity);
            onAnyGrenadeExploded?.Invoke();
            onGrenadeBehaviourComplete();
        }
    }
}
