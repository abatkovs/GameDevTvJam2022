using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private float _spawnRate = 1f;

    private void Start() {
        if(_gameManager == null) _gameManager = GameManager.Instance;
    }

    
}
