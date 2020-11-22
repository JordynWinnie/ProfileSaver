using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Profile", order = 1)]
public class Profile : ScriptableObject
{
    public string profileName;
    public double income;
    public double maxIncome;
    public int age;
    public Sprite profileIcon;

    public string description;

    public List<ScheduledEvent> mondayEvents;
    public List<ScheduledEvent> tuesdayEvents;
    public List<ScheduledEvent> wednesdayEvents;
    public List<ScheduledEvent> thursdayEvents;
    public List<ScheduledEvent> fridayEvents;
    public List<ScheduledEvent> saturdayEvents;
    public List<ScheduledEvent> sundayEvents;
}