using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ProfileSaver Object/Profile", order = 1)]
public class Profile : ScriptableObject
{
    public string profileName;
    public double income;
    public double maxIncome;
    public int age;
    public Sprite profileIcon;

    public string description;
}