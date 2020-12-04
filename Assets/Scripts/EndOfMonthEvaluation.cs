using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EndOfMonthEvaluation : MonoBehaviour
{
    [SerializeField] private StarDisplay healthDisplay;
    [SerializeField] private StarDisplay wealthDisplay;
    [SerializeField] private StarDisplay happinessDisplay;
    [SerializeField] private StarDisplay goalsDisplay;
    [SerializeField] private StarDisplay overallDisplay;

    private void Start()
    {
        healthDisplay.DisplayStar(CalculateHealth());
        wealthDisplay.DisplayStar(CalculateWealth());
        happinessDisplay.DisplayStar(CalculateHappiness());
        goalsDisplay.DisplayStar(CalculateGoals());
        var list = new List<int>
        {
            CalculateHealth(), CalculateWealth(), CalculateHappiness(), CalculateGoals()
        };

        overallDisplay.DisplayStar(CalculateOverall(list));
    }

    private int CalculateOverall(List<int> allVariables)
    {
        var total = 0f;
        for (int i = 0; i < allVariables.Count; i++)
        {
            total += allVariables[i];
        }

        return Mathf.FloorToInt(total / allVariables.Count);
    }

    private int CalculateHappiness()
    {
        var happiness = GameManager.instance.Happiness;
        print($"happiness: {happiness}");
        if (happiness <= 20)
        {
            return 1;
        }
        else if (happiness >= 21 && happiness <= 39)
        {
            return 2;
        }
        else if (happiness >= 40 && happiness <= 59)
        {
            return 3;
        }
        else if (happiness >= 60 && happiness <= 79)
        {
            return 4;
        }
        else
        {
            return 5;
        }
    }

    private int CalculateHealth()
    {
        var health = GameManager.instance.Health;
        print($"Health: {health}");
        if (health <= 29)
        {
            return 1;
        }
        else if (health >= 30 && health <= 49)
        {
            return 2;
        }
        else if (health >= 50 && health <= 69)
        {
            return 3;
        }
        else if (health >= 70 && health <= 84)
        {
            return 4;
        }
        else
        {
            return 5;
        }
    }

    private int CalculateWealth()
    {
        var currentWealth = GameManager.instance.Money;
        var wealthAtStartOfMonth = GameManager.instance.startMoneyOfMonth;
        GameManager.instance.startMoneyOfMonth = currentWealth;
        var percentage = currentWealth / wealthAtStartOfMonth * 100;

        print($"Wealth Calculation: CurrentWealth: {currentWealth} WealthAtStart: {wealthAtStartOfMonth} Percentage: {percentage}");

        if (percentage <= 25)
        {
            return 1;
        }
        else if (percentage >= 26 && percentage <= 49)
        {
            return 2;
        }
        else if (percentage >= 50 && percentage <= 79)
        {
            return 3;
        }
        else if (percentage >= 80 && percentage <= 100)
        {
            return 4;
        }
        else
        {
            return 5;
        }
    }

    private int CalculateGoals()
    {
        var completedGoalCount = GoalManager.instance.completeGoals.Where(x => x.goalType == GoalManager.GoalLength.Monthly).Count();
        var totalGoals = GameManager.instance.currentProfile.goals.Where(x => x.goalType == GoalManager.GoalLength.Monthly).Count();

        var percentageComplete = ((float)completedGoalCount / totalGoals) * 5f;

        return Mathf.RoundToInt(percentageComplete);
    }
}