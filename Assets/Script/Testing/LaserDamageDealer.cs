using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LaserDamageDealer : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float damagePerTick = 10f;
    [SerializeField] private float tickInterval = 0.2f;

    private readonly Dictionary<IDamageable, float> nextDamageTime = new();

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent<IDamageable>(out IDamageable damageable))
            return;

        ApplyDamageIfReady(damageable);
    }

    private void ApplyDamageIfReady(IDamageable damageable)
    {
        float currentTime = Time.time;

        if (nextDamageTime.TryGetValue(damageable, out float nextTime) && currentTime < nextTime)
            return;

        damageable.TakeDamage(damagePerTick);
        nextDamageTime[damageable] = currentTime + tickInterval;
    }

    private void OnDisable()
    {
        nextDamageTime.Clear();
    }
}