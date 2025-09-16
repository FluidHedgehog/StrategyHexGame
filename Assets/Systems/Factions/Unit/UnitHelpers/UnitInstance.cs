using UnityEngine;

public class UnitInstance : MonoBehaviour

{
    //------------------------------------------------------------------------------
    // Critical References
    //------------------------------------------------------------------------------

    public UnitData unitData;
    public Side side;
    public bool isActive;

    //------------------------------------------------------------------------------
    // Unit Core Values
    //------------------------------------------------------------------------------

    public int currentHealthPoints;
    public int currentExperiencePoints;
    public float currentActionPoints;
    public float luckPoints;

    //------------------------------------------------------------------------------
    // Unit currentPosition
    //------------------------------------------------------------------------------

    public Vector3Int currentTile;

    //------------------------------------------------------------------------------
    // Initialization of unit values
    //------------------------------------------------------------------------------

    void Awake()
    {
        currentHealthPoints = unitData.maxHealth;
        currentExperiencePoints = 0;
        currentActionPoints = unitData.maxActionPoints;
        luckPoints = unitData.luck;
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
        if (currentExperiencePoints >= unitData.maxExperience)
        {
            LevelUp();
        }
        OnStatsChanged?.Invoke();
    }

    public void Die()
    {

    }

    public void LevelUp()
    {
        currentExperiencePoints = 0;
        currentHealthPoints = unitData.maxHealth;
        currentActionPoints = unitData.maxActionPoints;
        // Additional logic for leveling up (e.g., increase stats, unlock abilities, etc.)
        OnStatsChanged?.Invoke();
    }

    public System.Action OnStatsChanged { get; set; }

    public void NotifyStatsChanged() {
        OnStatsChanged?.Invoke();
    }

}


