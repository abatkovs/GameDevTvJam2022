using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Target : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TargetType targetType;
    [SerializeField] private TargetStats targetStats;
    [SerializeField] private ParticleSystem destroyParticles;
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    [SerializeField] private float upForceMin = 10;
    [SerializeField] private float upForceMax = 16;
    [SerializeField] private float torqueForce = 5;
    [SerializeField] private float spawnAreaLimit = 20; // range in which i can spawn targets
    [SerializeField] private int pointValue = 5;
    [SerializeField] private float spawnOffset = -1; // offset bellow screen

    public event Action<Target> OnDeath;

    private void Start()
    {
        if(rb == null) rb = GetComponent<Rigidbody>();
        if(targetStats == null) targetStats = GetComponent<TargetStats>();

        gameManager = GameManager.Instance;

        rb.AddForce(RandomForce(upForceMin, upForceMax), ForceMode.Impulse);
        rb.AddTorque(RandomTorque(-torqueForce, torqueForce), RandomTorque(-torqueForce, torqueForce), RandomTorque(-torqueForce, torqueForce), ForceMode.Impulse);

        transform.position = RandomSpawnPos();
    }

    /// <summary>
    /// Generate random spawn position
    /// </summary>
    /// <returns></returns>
    private Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-spawnAreaLimit, spawnAreaLimit), spawnOffset);
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

    private void RandomForceOnClick(Vector3 force, float forceModifier = 0){
        rb.velocity = Vector3.zero;
        var upForce = 1;
        var shoveDir = new Vector3(force.x, upForce, force.z);
        //_rb.angularVelocity = Vector3.zero;
        //rb.AddForce(RandomForce(6, 18), ForceMode.Impulse);
        rb.AddForce(shoveDir * 10, ForceMode.Impulse);
    }

    private void DealDamageToTarget(int damageAmount){
        targetStats.TakeDamage(damageAmount);
        gameManager.AddScore(damageAmount);
        if(targetStats.IsDead){
            OnDeath?.Invoke(this);
            DestroyTarget();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var rayResult = eventData.pointerCurrentRaycast;
        var hitPoint = rayResult.worldPosition;
        var position = transform.position;
        SpawnHurtParticles(hitPoint);
        RandomForceOnClick(position - rayResult.worldPosition);
        DealDamageToTarget(gameManager.PlayerStats.Damage);
        audioSource.PlayOneShot(audioClip);
    }

    private void MoveInAir(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }
    private void SpawnHurtParticles(Vector3 pos){
        //Debug.Log($"Make hurt particles");
        Instantiate(hitParticles, pos, Quaternion.identity);
    }

    private void SpawnDeathParticles(Vector3 pos)
    {
        Instantiate(destroyParticles, pos, quaternion.identity);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("This triggers");
        if(targetType == TargetType.Zombie){
            //Do bad zombie stuff here
            gameManager.PlayerStats.TakeDamage(targetStats.Damage);
        }
        if(targetType == TargetType.Human){
            //Do human stuff her
        }
        if(targetType == TargetType.Food){
            //Do food stuff here
        }
        OnDeath?.Invoke(this);
        DestroyTarget();
        
    }

    public void DestroyTarget()
    {
        SpawnDeathParticles(transform.position);
        Destroy(gameObject);
    }
}

public enum TargetType{
    None,
    Zombie,
    Human,
    Food
}