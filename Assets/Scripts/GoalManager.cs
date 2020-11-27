using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public static GoalManager instance;

    public enum GoalType { Daily, Monthly }

    public enum TrackingType { StudyTime, WorkTime, MealBreakfast, MealLunch, MealDinner }

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
    }

    public string PrintGoals()
    {
        var goal = GameManager.instance.currentProfile.goals;
        var sb = new StringBuilder();
        sb.AppendLine("Daily Goals:");

        var dailyGoalList = goal.Where(x => x.goalType == GoalType.Daily);

        foreach (var dailyGoal in dailyGoalList)
        {
            var testVal = 0f;
            switch (dailyGoal.trackingType)
            {
                case TrackingType.StudyTime:
                    testVal = StudyTime;
                    break;

                case TrackingType.WorkTime:
                    testVal = WorkTime;
                    break;

                case TrackingType.MealBreakfast:
                    testVal = mealBreakfast;
                    break;

                case TrackingType.MealLunch:
                    testVal = mealLunch;
                    break;

                case TrackingType.MealDinner:
                    testVal = mealDinner;
                    break;

                default:
                    break;
            }
            sb.AppendLine($"- {dailyGoal.goalName} ({testVal}/{dailyGoal.totalCommitment})");
        }
        sb.AppendLine();
        var monthlyGoalList = goal.Where(x => x.goalType == GoalType.Monthly);

        sb.AppendLine("Monthly Goals:");
        foreach (var monthlyGoal in monthlyGoalList)
        {
            var testVal = 0f;
            switch (monthlyGoal.trackingType)
            {
                case TrackingType.StudyTime:
                    testVal = StudyTime;
                    break;

                case TrackingType.WorkTime:
                    testVal = WorkTime;
                    break;

                case TrackingType.MealBreakfast:
                    testVal = mealBreakfast;
                    break;

                case TrackingType.MealLunch:
                    testVal = mealLunch;
                    break;

                case TrackingType.MealDinner:
                    testVal = mealDinner;
                    break;

                default:
                    break;
            }
            sb.AppendLine($"- {monthlyGoal.goalName} ({testVal}/{monthlyGoal.totalCommitment})");
        }

        return sb.ToString();
    }
}