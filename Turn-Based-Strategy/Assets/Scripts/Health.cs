using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int startingHealth = 100;
    private int currentHealth;
    public Action OnDead;
    public Action OnDamageTaken;

    private void Awake()
    {
        currentHealth = startingHealth;
    }
    public bool TakeDamage(int damage)
    {
        if(currentHealth - damage > 0)
        {
            currentHealth -= damage;
            OnDamageTaken?.Invoke();
            return true;
        }
        else
        {
            OnDead?.Invoke();
            return false;
        }
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
