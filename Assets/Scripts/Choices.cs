using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Make New Decision/Choice", order = 1)]
public class Choices : ScriptableObject
{
    public string choiceName;
    public float happiness;
    public float health;
    public float money;
    public float hunger;
    public float energy;
    public float timeTaken;
}