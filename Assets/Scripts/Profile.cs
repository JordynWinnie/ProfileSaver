using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Profile", order = 1)]
public class Profile : ScriptableObject
{
    public string profileName;
    public double income;
    public double dailyIncome;
    public int age;
    public Sprite profileIcon;

    public string description;
}