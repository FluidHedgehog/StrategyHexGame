using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    [SerializeField] private UnitData unit;
    [SerializeField] private UnitInstance unitInstance;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider expBar;
    [SerializeField] private Slider actionPointsBar;

    private void Awake()
    {
        healthBar.maxValue = unit.maxHealth;
        expBar.maxValue = unit.maxExperience;
        actionPointsBar.maxValue = unit.maxActionPoints;
    }
    private void Start()
    {
        unitInstance.OnStatsChanged += UpdateUI;

        UpdateUI();
    }

    private void OnDestroy()
    {
        unitInstance.OnStatsChanged -= UpdateUI;
    }

    public void UpdateUI()
    {
        healthBar.value = unitInstance.currentHealthPoints;
        expBar.value = unitInstance.currentExperiencePoints;
        actionPointsBar.value = unitInstance.currentActionPoints;
    }

}