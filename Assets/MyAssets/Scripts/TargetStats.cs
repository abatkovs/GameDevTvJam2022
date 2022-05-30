using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetStats : MonoBehaviour
{
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int Damage { get; private set; } //damage dealt to player
    [field: SerializeField] public bool IsDead { get; private set; }

    public event Action<int> OnUnitDamaged;
    public event Action<int> OnUnitHealed;

    private void Start() {
        Health = MaxHealth;
    }

    public void TakeDamage(int damage){
        Health = Mathf.Clamp(Health - damage, 0, MaxHealth);
        if (Health <= 0)
        {
            IsDead = true;
        }
        OnUnitDamaged?.Invoke(damage);
    }

}
