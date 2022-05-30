using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetStats : MonoBehaviour
{
    private GameManager _gameManager;
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int Damage { get; private set; } //damage dealt to player
    [field: SerializeField] public bool IsDead { get; private set; }

    public event Action<int> OnUnitDamaged;
    public event Action<int> OnUnitHealed;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        SetZombieStats();
    }

    private void SetZombieStats()
    {
        var killedZombies = _gameManager.spawner.GetKilledZombies();
        if ( killedZombies > 0)
        {
            MaxHealth = MaxHealth + killedZombies;
            Damage = Damage + (killedZombies / 2);
        }

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
