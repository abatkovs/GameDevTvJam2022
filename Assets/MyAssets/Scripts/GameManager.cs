using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("UI")]
    [SerializeField] private GameObject _titleGrp;
    [SerializeField] private GameObject _gameOverGrp;
    [SerializeField] private GameObject _upgradeGrp;
    [SerializeField] private TMP_Text _scoreText;
    
    [Header("Targets")]
    public List<GameObject> targetPrefabs;

    [Header("Game stuff")]
    [SerializeField] private int _score;
    public bool isGameActive = true;

    private void Start() {
        Instance = this;
    }

    public void GameOver(){
        _gameOverGrp.SetActive(true);
        isGameActive = false;
    }

    public void Upgrades(){
        _upgradeGrp.SetActive(true);
        isGameActive = false;
    }

    public void AddScore(int scoreToAdd){
        _score += scoreToAdd;
        _scoreText.text = $"SCORE: {_score}";
    }

    public void StartGame(){
        _titleGrp.SetActive(false);
        isGameActive = true;
        _score = 0;
    }
}
