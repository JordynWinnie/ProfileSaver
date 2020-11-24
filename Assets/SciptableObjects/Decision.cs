using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "ProfileSaver Object/Decision", order = 2)]
public class Decision : ScriptableObject
{
    public string decisionTitle;
    public string decisionString;
    public Sprite decisionIcon;
    public List<Choices> availableChoices;
    public bool isGenericDecision;
    public List<Profile> profilesGetDecision;
}