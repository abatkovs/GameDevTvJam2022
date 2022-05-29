using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Just visual actual health logic is inside UnitStats
/// </summary>
public class HealthBar : MonoBehaviour
{
    [SerializeField] private TargetStats _unitStats;
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private Color _damagedColor;
    [SerializeField] private Color _healedColor;
    [SerializeField] private float _chipSpeed;
    [SerializeField] private float _lerpTimer;
    [SerializeField] private float _hideHealthBarAfter = 3f;

    [Header("visuals")]
    private float fillHP;
    private float fillChip;
    private float healthFraction;
    private float percentComplete;

    public GameObject visuals;
    public Image healthBar;
    public Image chipHealthBar;

    [SerializeField] private TMP_Text _damageTxt;
    [SerializeField] private TMP_Text _healthTxt;

    [SerializeField] private GameObject _targetBody;
    [SerializeField] private Vector3 _offset = new Vector3(3, 0, 0);

    private void LateUpdate()
    {
        transform.position = new Vector3(_targetBody.transform.position.x, _targetBody.transform.position.y, 0) + _offset;
    }

    private void Start() {
        visuals.SetActive(false);
        _unitStats.OnUnitDamaged += TakeDamage;
        _unitStats.OnUnitHealed += TargetHealed;
        _health = _unitStats.Health;
        _maxHealth = _unitStats.MaxHealth;
        _health = _maxHealth;
        _damageTxt.text = $"{_unitStats.Damage}";
        _healthTxt.text = $"{_unitStats.Health}";
    }



    private void Update() {
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI(){
        fillHP = healthBar.fillAmount;
        fillChip = chipHealthBar.fillAmount;
        healthFraction = (float)_health / _maxHealth;

        if(_health != _maxHealth || _health <= 0) {
            visuals.SetActive(true);
        }

        if(fillChip > healthFraction) {
            healthBar.fillAmount = healthFraction;
            _lerpTimer += Time.deltaTime;
            percentComplete = _lerpTimer / _chipSpeed;
            percentComplete = percentComplete * percentComplete;
            chipHealthBar.fillAmount = Mathf.Lerp(fillChip, healthFraction, percentComplete);
        }

        if(fillHP < healthFraction){
            chipHealthBar.fillAmount = healthFraction;
            _lerpTimer += Time.deltaTime;
            percentComplete = _lerpTimer /_chipSpeed;
            percentComplete = percentComplete * percentComplete;
            healthBar.fillAmount = Mathf.Lerp(fillHP, chipHealthBar.fillAmount, percentComplete);
        }

    }

    IEnumerator DisableVisuals(){
        yield return new WaitForSeconds(_hideHealthBarAfter);
        visuals.SetActive(false);
    }

    private void UnitRevived()
    {
        visuals.SetActive(true);
        _health = _unitStats.Health;
        _maxHealth = _unitStats.MaxHealth;
        _health = _maxHealth;
        chipHealthBar.color = _healedColor;
        _lerpTimer = 0f;
        StartCoroutine("DisableVisuals");
    }
    private void TargetHealed(int healAmount)
    {
        HealDamage(healAmount);
    }

    public void TakeDamage(int damage){
        
        visuals.SetActive(true);
        chipHealthBar.color = _damagedColor;
        _health -= Mathf.Abs(damage);
        _lerpTimer = 0f;
        UpdateHealthValue();
    }

    public void HealDamage(int healAmount){
        _health += healAmount;
        chipHealthBar.color = _healedColor;
        _lerpTimer = 0f;
        UpdateHealthValue();
    }

    private void UpdateHealthValue(){
        _healthTxt.text = $"{Mathf.Clamp(_health, 0, _unitStats.MaxHealth)}";
    }

    private void OnDestroy() {
        _unitStats.OnUnitDamaged -= TakeDamage;
        _unitStats.OnUnitHealed -= TargetHealed;
    }

    [ContextMenu("Deal Damage")]
    public void Test(){
        TakeDamage(10);
    }

    [ContextMenu("Heal Damage")]
    public void Test2(){
        HealDamage(10);
    }
}
