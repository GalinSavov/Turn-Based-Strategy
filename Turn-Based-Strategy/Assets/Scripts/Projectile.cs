using Game.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 targetPosition;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private TrailRenderer trailRenderer = null;
    [SerializeField] private ParticleSystem bulletHitExplosionEffect = null;
    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
    public void MoveToTarget()
    {
        float distanceBeforeMoving = Vector3.Distance(targetPosition,transform.position); 

        Vector3 moveDirection = (this.targetPosition - transform.position).normalized;
        transform.position += moveDirection * Time.deltaTime * projectileSpeed;

        float distanceAfterMoving =Vector3.Distance(targetPosition,transform.position);

        if (distanceAfterMoving > distanceBeforeMoving) //check if the projectile went through the target
        {
            transform.position = targetPosition;
            trailRenderer.transform.parent = null;
            Destroy(gameObject);
            Instantiate(bulletHitExplosionEffect,targetPosition,Quaternion.identity);
        }
    }
    private void Update()
    {
        if (this.targetPosition == null) return;
            MoveToTarget();
    }
}
