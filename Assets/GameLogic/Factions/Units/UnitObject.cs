using UnityEngine;

public class UnitObject : MonoBehaviour
{
    public Unit unitData;

    public int level;
    public int currentHealthPoints;
    public int currentExperiencePoints;

    public float currentActionPoints;
    public float luckPoints;

    public bool isSelected;
    public bool isAlive;

    public Vector3Int currentTile;

    void Awake()
    {
        isAlive = true;
        isSelected = false;
        currentHealthPoints = unitData.healthPoints;
        currentExperiencePoints = 0;
        currentActionPoints = unitData.maxActionPoints;
        luckPoints = unitData.baseLuckPoints;
    }

    public void TakeDamage(int amount)
    {
        currentHealthPoints -= amount;
        if (currentHealthPoints <= 0)
        {
            Die();
        }
        OnStatsChanged?.Invoke();
    }

    public void ToMove(float amount)
    {
        if (currentActionPoints >= amount)
        {
            currentActionPoints -= amount;
            OnStatsChanged?.Invoke();
        }
    }

    public void ToAttack(float amount)
    {
        if (currentActionPoints >= amount)
        {
            currentActionPoints -= amount;
            OnStatsChanged?.Invoke();
        }
    }

    public void GainExperience(int amount)
    {
        currentExperiencePoints += amount;
        if (currentExperiencePoints >= unitData.maxExperiencePoints)
        {
            LevelUp();
        }
        OnStatsChanged?.Invoke();
    }

    public void Die()
    {
        isAlive = false;
    }

    public void LevelUp()
    {
        level++;
        currentExperiencePoints = 0;
        currentHealthPoints = unitData.healthPoints;
        currentActionPoints = unitData.maxActionPoints;
        // Additional logic for leveling up (e.g., increase stats, unlock abilities, etc.)
        OnStatsChanged?.Invoke();
    }

    public event System.Action OnStatsChanged;

}
