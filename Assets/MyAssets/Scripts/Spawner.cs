using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private float _spawnRate = 10;


    [Header("Targets")]
    public List<GameObject> spawnableTargets;
    public List<Target> spawnedTargets;

    private void Start() {
        if(_gameManager == null) _gameManager = GameManager.Instance;
    }

    public void StartSpawningTargets(){
        StartCoroutine("SpawnTargets");
    }

    IEnumerator SpawnTargets()
    {
        while (_gameManager.isGameActive)
        {
            var i = Random.Range(0, spawnableTargets.Count); //Select radom target
            var targetGo = Instantiate(spawnableTargets[i]);
            var target = targetGo.GetComponent<Target>();
            AddSpawnedTarget(target);
            yield return new WaitForSeconds(_spawnRate);
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

}
