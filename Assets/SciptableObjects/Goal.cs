using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ProfileSaver Object/Goal", order = 1)]
public class Goal : ScriptableObject
{
    public string goalName;
    public GoalManager.GoalType goalType;
    public GoalManager.TrackingType trackingType;
    public float totalCommitment;
}