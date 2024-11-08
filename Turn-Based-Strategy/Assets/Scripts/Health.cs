using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int startingHealth = 100;
    private int currentHealth;
    public Action OnDead;

    private void Awake()
    {
        currentHealth = startingHealth;
    }
    public bool TakeDamage(int damage)
    {
        if(currentHealth - damage > 0)
        {
            currentHealth -= damage;
            return true;
        }
        else
        {
            OnDead?.Invoke();
            return false;
        }
    }
}
