using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ScreenShake : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulseSource = null;
    public static ScreenShake Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void Shake(float intensity)
    {
        impulseSource.GenerateImpulse(intensity);
    }
}
