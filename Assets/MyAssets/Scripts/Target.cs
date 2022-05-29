using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Target : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TargetType _targetType;
    [SerializeField] private TargetStats _targetStats;
    [SerializeField] private ParticleSystem _destroyParticles;
    [SerializeField] private GameObject _testPF;

    [SerializeField] private float _upForceMin = 10;
    [SerializeField] private float _upForceMax = 16;
    [SerializeField] private float _torqueForce = 5;
    [SerializeField] private float _spawnAreaLimit = 4; // range in which i can spawn targets
    [SerializeField] private int _pointValue = 5;
    [SerializeField] private float _spawnOffset = -1; // offset bellow screen

    public event Action<Target> OnDeath;

    private void Start()
    {
        if(_rb == null) _rb = GetComponent<Rigidbody>();
        if(_targetStats == null) _targetStats = GetComponent<TargetStats>();

        _gameManager = GameManager.Instance;

        _rb.AddForce(RandomForce(_upForceMin, _upForceMax), ForceMode.Impulse);
        _rb.AddTorque(RandomTorque(-_torqueForce, _torqueForce), RandomTorque(-_torqueForce, _torqueForce), RandomTorque(-_torqueForce, _torqueForce), ForceMode.Impulse);

        transform.position = RandomSpawnPos();
    }

    /// <summary>
    /// Generate random spawn position
    /// </summary>
    /// <returns></returns>
    private Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-_spawnAreaLimit, _spawnAreaLimit), _spawnOffset);
    }

    /// <summary>
    /// Give random rotation to target
    /// </summary>
    /// <returns></returns>
    private float RandomTorque(float torqueMin, float torqueMax)
    {
        return Random.Range(torqueMin, torqueMax);
    }

    /// <summary>
    /// Give random force to targets
    /// </summary>
    /// <returns></returns>
    private Vector3 RandomForce(float forceMin, float forceMax)
    {
        return Vector3.up * Random.Range(forceMin, forceMax);
    }

    private void RandomForceOnClick(float forceModifier = 0){
        _rb.velocity = Vector3.zero;
        //_rb.angularVelocity = Vector3.zero;
        _rb.AddForce(RandomForce(6, 18), ForceMode.Impulse);
    }

    private void DealDamage(int damageAmount){
        _targetStats.DealDamage(damageAmount);
        _gameManager.AddScore(damageAmount);
        RandomForceOnClick();
        if(_targetStats.IsDead){
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked on: {transform.name}");
        //TODO: particles for clicking and destroying
        Debug.Log($"Click Event: {eventData.position}");
        Instantiate(_testPF, eventData.position, Quaternion.identity);
        DealDamage(2);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("This triggers");
        if(_targetType == TargetType.Zombie){
            //Do bad zombie stuff here
        }
        if(_targetType == TargetType.Human){
            //Do human stuff her
        }
        if(_targetType == TargetType.Food){
            //Do food stuff here
        }
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }
}

public enum TargetType{
    None,
    Zombie,
    Human,
    Food
}