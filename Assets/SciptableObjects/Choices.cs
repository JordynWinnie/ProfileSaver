using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ProfileSaver Object/Choice", order = 1)]
public class Choices : ScriptableObject
{
    public string choiceName;
    public float happinessToAdd;
    public float healthToAdd;

    [FormerlySerializedAs("money")]
    public float moneyToAdd;

    public float hunger;
    public float energy;
    public float timeTaken;
}