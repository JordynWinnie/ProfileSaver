using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public static GoalManager instance;

    public enum GoalLength { Daily, Monthly }

    public int ActionsTaken { get; set; }
    public float StudyTime { get; set; }
    public float WorkTime { get; set; }
    public int ItemsPurchased { get; set; }
    public int MealsEaten { get; set; }

    public int mealBreakfast { get; set; }
    public int mealLunch { get; set; }
    public int mealDinner { get; set; }

    [SerializeField] private List<Stat> trackedStatistics;
    private List<Goal> incompleteGoals = new List<Goal>();
    private List<Goal> completeGoals = new List<Goal>();

    // Start is called before the first frame update
    private void Awake()
    {
        trackedStatistics = new List<Stat>();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (var item in trackedStatistics)
            {
                print($"{item.statType}: Visted: {item.placeVisited} - Time: {item.timeOfAction} - Day: {item.dayOfAction} - ActionContributes: {item.actionCount} - MiscParams: {item.miscStatParams}");
            }
        }
    }

    public Stat AddStat(Stat stat)
    {
        trackedStatistics.Add(stat);
        return stat;
    }

    public string PrintGoals()
    {
        completeGoals.Clear();
        incompleteGoals.Clear();
        var currentGameTime = GameManager.instance.gameTime;
        var goalList = GameManager.instance.currentProfile.goals;
        var sb = new StringBuilder();

        var goalGroup = from x in goalList
                        orderby x.goalType
                        group x by x.goalType into y
                        select y;

        foreach (var timing in goalGroup)
        {
            sb.AppendLine($"{timing.Key} Goals:");
            foreach (var goal in timing)
            {
                var statList = trackedStatistics.Where(x => x.statType == goal.statType);
                if (!goal.miscStatParams.Equals(string.Empty))
                {
                    statList = statList.Where(x => x.miscStatParams == goal.miscStatParams);
                }
                switch (goal.goalType)
                {
                    case GoalLength.Daily:
                        statList = statList.Where(x => x.dayOfAction == currentGameTime.ReturnDayNumber());
                        if (!goal.acceptAllTime)
                        {
                            statList = statList.Where(x => x.timeOfAction >= goal.onlyAcceptFromTime && x.timeOfAction <= goal.onlyAcceptToTime);
                        }

                        break;

                    case GoalLength.Monthly:
                        var monthNumber = (int)((currentGameTime.ReturnDayNumber() - 1) / 30) + 1;
                        var monthEndDay = monthNumber * 30;
                        var monthStartDay = monthEndDay - 29;
                        statList = statList.Where(x => x.dayOfAction >= monthStartDay && x.dayOfAction <= monthEndDay);
                        if (!goal.acceptAllTime)
                        {
                            statList = statList.Where(x => x.timeOfAction >= goal.onlyAcceptFromTime && x.timeOfAction <= goal.onlyAcceptToTime);
                        }
                        break;

                    default:
                        break;
                }
                var amountCompleted = statList.Sum(x => x.actionCount);
                if (amountCompleted >= goal.totalCommitment)
                {
                    completeGoals.Add(goal);
                    sb.AppendLine($"- {goal.goalName} ({Mathf.Clamp(amountCompleted, 0f, goal.totalCommitment)}/{goal.totalCommitment}) (COMPLETE)");
                }
                else
                {
                    incompleteGoals.Add(goal);
                    sb.AppendLine($"- {goal.goalName} ({Mathf.Clamp(amountCompleted, 0f, goal.totalCommitment)}/{goal.totalCommitment}) (INCOMPLETE)");
                }
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}