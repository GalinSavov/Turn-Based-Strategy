using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health unitHealth = null;
    [SerializeField] private Image healthBar = null;
    private void Awake()
    {
        unitHealth.OnDamageTaken += HandleOnDamageTaken;

    }
    private void Start()
    {
        UpdateHealthBar();
    }
    private void OnDestroy()
    {
        unitHealth.OnDamageTaken -= HandleOnDamageTaken;
    }
    private void UpdateHealthBar()
    {
        healthBar.fillAmount = unitHealth.GetCurrentHealth() / 100f;
    }

    private void HandleOnDamageTaken()
    {
        UpdateHealthBar();
    }
}
