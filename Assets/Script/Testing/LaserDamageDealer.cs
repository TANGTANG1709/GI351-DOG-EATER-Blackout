using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LaserDamageDealer : MonoBehaviour
{
    [SerializeField] private float damagePerTick = 10f;
    [SerializeField] private float tickInterval = 0.2f; // re-damage same target every 0.2s while it's held on them

    private readonly Dictionary<IDamageable, float> nextTickTime = new();

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            if (!nextTickTime.TryGetValue(damageable, out float nextTime) || Time.time >= nextTime)
            {
                damageable.TakeDamage(damagePerTick);
                nextTickTime[damageable] = Time.time + tickInterval;
            }
        }
    }

    private void OnDisable()
    {
        nextTickTime.Clear(); // reset ticks so re-triggering deals damage immediately next swing
    }
}