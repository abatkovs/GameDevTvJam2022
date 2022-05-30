using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float spawnRate = 1;


    [Header("Targets")]
    public List<GameObject> spawnableTargets;
    public List<Target> spawnedTargets;

    private void Start()
    {
        if(gameManager == null) gameManager = GameManager.Instance;
        gameManager.OnGameOver += ClearScene;
        PlayerStats.OnUpdateInterval += UpdateSpawnInterval;
    }

    private void UpdateSpawnInterval(int intervalTime)
    {
        spawnRate = intervalTime;
    }

    private void ClearScene()
    {
        foreach (var target in spawnedTargets)
        {
            target.DestroyTarget();
        }
        spawnedTargets.Clear();
    }

    public void StartSpawningTargets(){
        StartCoroutine(nameof(SpawnTargets));
    }

    private IEnumerator SpawnTargets()
    {
        while (gameManager.isGameActive)
        {
            var i = Random.Range(0, spawnableTargets.Count); //Select radom target
            var targetGo = Instantiate(spawnableTargets[i]);
            var target = targetGo.GetComponent<Target>();
            AddSpawnedTarget(target);
            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void AddSpawnedTarget(Target target){
        spawnedTargets.Add(target);
        target.OnDeath += TargetDied;
    }

    private void TargetDied(Target target)
    {
        RemoveSpawnedTarget(target);
    }

    public void RemoveSpawnedTarget(Target target){
        target.OnDeath -= TargetDied;
        spawnedTargets.Remove(target);
    }

    private void OnDestroy()
    {
        PlayerStats.OnUpdateInterval -= UpdateSpawnInterval;
    }
}
