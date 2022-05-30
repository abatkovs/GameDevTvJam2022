using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField] private int health;
    [field: SerializeField] public int MAXHealth { get; private set; } = 100;
    [field: SerializeField] public int Damage { get; private set; }
    [SerializeField] private int power;
    [SerializeField] private int hpRegen;
    [SerializeField] private int spawnInterval;
    [field: SerializeField] public bool IsAlive { get; private set;}

    [Header("Visuals")]
    [SerializeField] private Image healthUI;
    [SerializeField] private Color damagedColor;
    [SerializeField] private Color healedColor;
    [SerializeField] private float chipSpeed;
    [SerializeField] private float lerpTimer;
    private float _fillHp;
    private float _fillChip;
    private float _healthFraction;
    private float _percentComplete;
    [SerializeField] private GameObject visuals;
    public Image healthBar;
    public Image chipHealthBar;

    [Header("PowerUps")] 
    [SerializeField] private int priceDmg;
    [SerializeField] private TMP_Text dmgTxt;
    [SerializeField] private TMP_Text dmgTxtPrice;
    [SerializeField] private int priceHp;
    [SerializeField] private TMP_Text hpTxt;
    [SerializeField] private TMP_Text hpTxtPrice;
    [SerializeField] private int pricePower;
    [SerializeField] private TMP_Text powerTxt;
    [SerializeField] private TMP_Text powerTxtPrice;
    [SerializeField] private int priceRegen;
    [SerializeField] private TMP_Text regenTxt;
    [SerializeField] private TMP_Text regenTxtPrice;
    [SerializeField] private int priceInterval;
    [SerializeField] private TMP_Text intervalTxt;
    [SerializeField] private TMP_Text intervalTxtPrice;
    
    [SerializeField] private float healInterval = 5f;
    private float _healTimer;

    public static event Action<int> OnUpdateInterval;
    
    private void Start() {
        _gameManager = GetComponent<GameManager>();
        health = MAXHealth;
        _healTimer = healInterval;
    }

    private void Update() {
        UpdateHealthUI();
        _healTimer -= Time.deltaTime;
        if (_healTimer <= 0)
        {
            HealDamage(hpRegen);
            _healTimer = healInterval;
        }

    }

    internal void TakeDamage(int takeDamage){
        Debug.Log($"Damage > {takeDamage} : {health} : {health - takeDamage}");
        health = Mathf.Clamp(health - takeDamage, 0, MAXHealth);
        lerpTimer = 0f;
        if (health > 0) return;
        IsAlive = false;
        _gameManager.GameOverScreen();
    }

    public void HealDamage(int amountToHeal)
    {
        health = Mathf.Clamp(health + amountToHeal, 0, MAXHealth);
    }

    private void UpdateHealthUI()
    {
        _fillHp = healthBar.fillAmount;
        _fillChip = chipHealthBar.fillAmount;
        _healthFraction = (float)health / MAXHealth;

        if (_fillChip > _healthFraction)
        {
            healthBar.fillAmount = _healthFraction;
            lerpTimer += Time.deltaTime;
            _percentComplete = lerpTimer / chipSpeed;
            _percentComplete = _percentComplete * _percentComplete;
            chipHealthBar.fillAmount = Mathf.Lerp(_fillChip, _healthFraction, _percentComplete);
        }

        if (_fillHp < _healthFraction)
        {
            chipHealthBar.fillAmount = _healthFraction;
            lerpTimer += Time.deltaTime;
            _percentComplete = lerpTimer / chipSpeed;
            _percentComplete = _percentComplete * _percentComplete;
            healthBar.fillAmount = Mathf.Lerp(_fillHp, chipHealthBar.fillAmount, _percentComplete);
        }
    }

    public void ToggleHealthBar(bool isVisible){
        visuals.SetActive(isVisible);
    }
//TODO: just improve make more generic
    public void IncreaseDamage()
    {
        int dmgPriceMulti = 10;
        if (priceDmg > _gameManager.Score) return;
        Damage += 1;
        _gameManager.RemoveScore(priceDmg);
        priceDmg = Damage * dmgPriceMulti;
        dmgTxt.text = $"Amount: {Damage}";
        dmgTxtPrice.text = $"Price: {priceDmg}";
    }

    public void IncreaseHealth()
    {
        int hpPriceMulti = 5;
        if (priceHp > _gameManager.Score) return;
        MAXHealth += 10;
        _gameManager.RemoveScore(priceHp);
        priceHp = MAXHealth * hpPriceMulti;
        hpTxt.text = $"Amount: {MAXHealth}";
        hpTxtPrice.text = $"Price: {priceHp}";
    }

    public void IncreasePower()
    {
        int powerPriceMulti = 2000;
        if(pricePower > _gameManager.Score) return;
        power += 1;
        _gameManager.RemoveScore(pricePower);
        pricePower = power * powerPriceMulti;
        powerTxt.text = $"Amount: {power}";
        powerTxtPrice.text = $"Price: {pricePower}";
    }

    public void IncreaseRegen()
    {
        int regenPriceMulti = 2000;
        if (priceRegen > _gameManager.Score) return;
        hpRegen += 1;
        _gameManager.RemoveScore(priceRegen);
        priceRegen = hpRegen * regenPriceMulti;
        regenTxt.text = $"Amount: {hpRegen}";
        regenTxtPrice.text = $"Price: {priceRegen}";
    }

    public void IncreaseInterval()
    {
        int intervalPriceMulti = 100;
        if (priceInterval > _gameManager.Score) return;
        spawnInterval += 1;
        _gameManager.RemoveScore(priceInterval);
        priceInterval = spawnInterval * intervalPriceMulti;
        OnUpdateInterval?.Invoke(spawnInterval);
        intervalTxt.text = $"Amount: {spawnInterval}s";
        intervalTxtPrice.text = $"Price: {priceInterval}";
    }
}
