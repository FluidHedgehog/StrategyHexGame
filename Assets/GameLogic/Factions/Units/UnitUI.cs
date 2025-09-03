using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private UnitObject unitObject;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider expBar;
    [SerializeField] private Slider actionPointsBar;

    private void Awake()
    {
        healthBar.maxValue = unit.healthPoints;
        expBar.maxValue = unit.maxExperiencePoints;
        actionPointsBar.maxValue = unit.maxActionPoints;
    }
    private void Start()
    {
        unitObject.OnStatsChanged += UpdateUI;

        UpdateUI();
    }

    private void OnDestroy()
    {
        unitObject.OnStatsChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        healthBar.value = unitObject.currentHealthPoints;
        expBar.value = unitObject.currentExperiencePoints;
        actionPointsBar.value = unitObject.currentActionPoints;
    }

}