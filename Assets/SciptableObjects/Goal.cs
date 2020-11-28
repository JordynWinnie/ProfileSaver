using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ProfileSaver Object/Goal", order = 0)]
public class Goal : ScriptableObject
{
    public string goalName;

    //Sets which stat to check:
    public Stat.StatType statType;

    //Sets length of tracking:
    public GoalManager.GoalLength goalType;

    public float totalCommitment;

    public bool acceptAllTime = true;
    public float onlyAcceptFromTime;
    public float onlyAcceptToTime;

    public string miscStatParams;
}