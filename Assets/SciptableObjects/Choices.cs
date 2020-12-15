using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ProfileSaver Object/Choice", order = 1)]
public class Choices : ScriptableObject
{
    public string choiceName;
    public float happinessToAdd;
    public float healthToAdd;

    [FormerlySerializedAs("money")] public float moneyToAdd;

    public float hunger;
    public float timeTaken;
    public Stat.StatType statType;
    public float progressionForStat;

    public int meatAmount;
    public int vegetableAmount;
    public int fishAmount;
    public int fruitAmount;
    
    public string miscStatParams;
}