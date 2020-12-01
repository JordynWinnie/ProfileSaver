using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ProfileSaver Object/Profile", order = 1)]
public class Profile : ScriptableObject
{
    public string profileName;
    public float income;
    public float maxIncome;
    public int age;
    public float timeToWake;
    public float timeToSleep;
    public Sprite profileIcon;

    public string description;

    public List<Goal> goals;
    
}