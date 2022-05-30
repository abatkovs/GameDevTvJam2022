using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("UI")]
    [SerializeField] private GameObject titleGrp;
    [SerializeField] private GameObject gameOverGrp;
    [SerializeField] private GameObject upgradeGrp;
    [Header("InGame")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text scoreTextUpg;
    [Header("Upgrade")] 
    [SerializeField] private TMP_Text upgradesText;
    [Header("Ref")]
    [SerializeField] public Spawner spawner;

    [Header("Game stuff")]
    public bool isGameActive = true;
    [field: SerializeField] public int Score { get; private set; }
    public float defaultTimeScale = 1f;
    public Vector3 defaultGravity;
    [field: SerializeField] public PlayerStats PlayerStats { get; private set;}

    public event Action OnGameOver;

    private void Start()
    {
        defaultGravity = Physics.gravity;
        defaultTimeScale = Time.timeScale;
        Instance = this;
        DisableAllUIGroups();
        titleGrp.SetActive(true);
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void ResetTimeScale()
    {
        Time.timeScale = defaultTimeScale;
    }

    public void SetGravity(Vector3 newGravity)
    {
        Physics.gravity = newGravity;
    }

    private void DisableAllUIGroups(){
        titleGrp.SetActive(false);
        gameOverGrp.SetActive(false);
        upgradeGrp.SetActive(false);
    }

    public void GameOverScreen(){
        DisableAllUIGroups();
        gameOverGrp.SetActive(true);
        isGameActive = false;
        OnGameOver?.Invoke();
    }

    public void UpgradesScreen(){
        DisableAllUIGroups();
        upgradeGrp.SetActive(true);
        isGameActive = false;
    }

    public void AddScore(int scoreToAdd)
    {
        Score += scoreToAdd;
        UpdateScoreTxt();
    }

    private void UpdateScoreTxt()
    {
        scoreText.text = $"SCORE: {Score}";
        scoreTextUpg.text = $"SCORE: {Score}";
    }

    public void RemoveScore(int scoreToRemove)
    {
        Score -= scoreToRemove;
        UpdateScoreTxt();
    }

    public void StartGame()
    {
        DisableAllUIGroups();
        isGameActive = true;
        Score = 0;
        UpdateScoreTxt();
        spawner.StartSpawningTargets();
    }


    public void ResetGame(){
        SceneManager.LoadScene(0);
    }

    public void BuyUpgrades(){
        UpgradesScreen();
    }

    public void ContinueGame()
    {
        //Reset zombie stats
        //Heal player
        StartGame();
        PlayerStats.HealDamage(PlayerStats.MAXHealth);
        spawner.ResetStats();
    }
}
