using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Make New Event/Event", order = 1)]
public class ScheduledEvent : ScriptableObject
{
    public string eventName;
    public float timeForEvent;
    public bool isHouseUnlocked;
    public bool isWorkplaceUnlocked;
    public bool isSchoolUnlocked;
    public bool isSupermarketUnlocked;
    public bool isResturantUnlocked;
}