using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ProfileSaver Object/Decision", order = 2)]
public class Decision : ScriptableObject
{
    public string decisionTitle;
    public string decisionString;
    public Sprite decisionIcon;
    public List<Choices> availableChoices;
    public bool isGenericDecision;
    public List<Profile> profilesGetDecision;

    public float startTimeToOccur;
    public float endTimeToOccur;
}