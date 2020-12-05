using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public enum GoalLength
    {
        Daily,
        Monthly
    }

    public static GoalManager instance;

    public List<Stat> trackedStatistics;
    public List<Goal> incompleteGoals = new List<Goal>();
    public List<Goal> completeGoals = new List<Goal>();

    public int ActionsTaken { get; set; }
    public float StudyTime { get; set; }
    public float WorkTime { get; set; }
    public int ItemsPurchased { get; set; }
    public int MealsEaten { get; set; }

    public int mealBreakfast { get; set; }
    public int mealLunch { get; set; }
    public int mealDinner { get; set; }

    // Start is called before the first frame update
    private void Awake()
    {
        trackedStatistics = new List<Stat>();
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateGoals();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            foreach (var item in trackedStatistics)
                print(
                    $"{item.statType}: Visted: {item.placeVisited} - Time: {item.timeOfAction} - Day: {item.dayOfAction} - ActionContributes: {item.actionCount} - MiscParams: {item.miscStatParams}");

        if (Input.GetKeyDown(KeyCode.G))
        {
            foreach (var item in completeGoals) print($"Complete: {item.goalName}");

            foreach (var item in incompleteGoals) print($"Incomplete: {item.goalName}");
        }

        UpdateGoals();
    }

    public Stat AddStat(Stat stat)
    {
        trackedStatistics.Add(stat);
        SaveSystem.SaveData(GameManager.instance);
        return stat;
    }

    public string UpdateGoals()
    {
        GameManager.instance.statList = instance.trackedStatistics;
        var currentGameTime = GameManager.instance.gameTime;
        var goalList = GameManager.instance.currentProfile.goals;
        var sb = new StringBuilder();

        var goalGroup = from x in goalList
            orderby x.goalType
            group x by x.goalType
            into y
            select y;

        foreach (var timing in goalGroup)
        {
            sb.AppendLine($"{timing.Key} Goals:");
            foreach (var goal in timing)
            {
                var statList = trackedStatistics.Where(x => x.statType == goal.statType);
                if (!goal.miscStatParams.Equals(string.Empty))
                    statList = statList.Where(x => x.miscStatParams == goal.miscStatParams);
                switch (goal.goalType)
                {
                    case GoalLength.Daily:
                        statList = statList.Where(x => x.dayOfAction == currentGameTime.ReturnDayNumber());
                        if (!goal.acceptAllTime)
                            statList = statList.Where(x =>
                                x.timeOfAction >= goal.onlyAcceptFromTime && x.timeOfAction <= goal.onlyAcceptToTime);

                        break;

                    case GoalLength.Monthly:
                        var monthNumber = (currentGameTime.ReturnDayNumber() - 1) / 30 + 1;
                        var monthEndDay = monthNumber * 30;
                        var monthStartDay = monthEndDay - 29;
                        statList = statList.Where(x => x.dayOfAction >= monthStartDay && x.dayOfAction <= monthEndDay);
                        if (!goal.acceptAllTime)
                            statList = statList.Where(x =>
                                x.timeOfAction >= goal.onlyAcceptFromTime && x.timeOfAction <= goal.onlyAcceptToTime);
                        break;
                }

                var amountCompleted = statList.Sum(x => x.actionCount);
                if (amountCompleted >= goal.totalCommitment)
                {
                    if (!completeGoals.Contains(goal))
                    {
                        completeGoals.Add(goal);
                        incompleteGoals.Remove(goal);

                        AlertDialog.instance.ShowAlert($"You completed: {goal.goalName}",
                            AlertDialog.AlertLength.Length_Long, AlertDialog.AlertType.Message);
                    }

                    sb.AppendLine(
                        $"- {goal.goalName} ({Mathf.Clamp(amountCompleted, 0f, goal.totalCommitment)}/{goal.totalCommitment}) (COMPLETE)");
                }
                else
                {
                    if (!incompleteGoals.Contains(goal))
                    {
                        completeGoals.Remove(goal);
                        incompleteGoals.Add(goal);
                    }

                    sb.AppendLine(
                        $"- {goal.goalName} ({Mathf.Clamp(amountCompleted, 0f, goal.totalCommitment)}/{goal.totalCommitment}) (INCOMPLETE)");
                }
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}