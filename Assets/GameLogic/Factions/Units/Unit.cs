using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Objects/Unit")]
public class Unit : ScriptableObject
{

    [SerializeField] string unitName;
    [SerializeField] string type;
    [SerializeField] string description;

    public enum MovementType { Walk, Swim, Fly };
    public MovementType movementType;

    public int healthPoints;

    [SerializeField] int strengthPoints;
    [SerializeField] int agilityPoints;
    [SerializeField] int defensePoints;

    public int maxExperiencePoints;

    [SerializeField] int GoldCost;
    [SerializeField] int GoldUpkeep;
    [SerializeField] int FoodCost;
    [SerializeField] int FoodUpkeep;
    [SerializeField] int MetalCost;
    [SerializeField] int fuelCost;
    [SerializeField] int fuelUpkeep;
    [SerializeField] int crystalCost;
    [SerializeField] int crystalUpkeep;


    public float maxActionPoints;
    public float baseLuckPoints;




    [SerializeField] List<PrefabAssetType> advancesTo;

    //[SerializeField] List<Abilities> abilities;

}
