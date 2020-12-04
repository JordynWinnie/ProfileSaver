using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisplayInformation : MonoBehaviour
{
    public static DisplayInformation infoDisplayHelper;
    public LocationInformation currentOpenLocation = null;
    public Location currentLocation = null;
    public List<Location> locationList;

    private void Awake()
    {
        if (infoDisplayHelper == null)
        {
            infoDisplayHelper = this;
        }
        else
        {
            Destroy(gameObject);
        }

        CloseAllPopups();
    }

    [SerializeField] private GameObject choiceButton;
    [SerializeField] private TextMeshProUGUI timeUI;
    [SerializeField] private TextMeshProUGUI moneyUI;
    [SerializeField] private TextMeshProUGUI healthUI;
    [SerializeField] private TextMeshProUGUI happinessUI;

    [SerializeField] private Image energyIcon;
    [SerializeField] private Image happinessIcon;

    [SerializeField] private TextMeshProUGUI hungerUI;
    [SerializeField] private TextMeshProUGUI energyUI;

    [SerializeField] private Image profileIcon;
    [SerializeField] private TextMeshProUGUI profileName;

    [SerializeField] private TextMeshProUGUI decisionStringUI;
    [SerializeField] private Image decisionIcon;
    [SerializeField] private GameObject choicesUI;

    [SerializeField] private RectTransform situationPopup;
    [SerializeField] private RectTransform placePopup;
    [SerializeField] private TextMeshProUGUI placeName;
    [SerializeField] private Image backgroundOfPlace;
    [SerializeField] private RectTransform blackFade;

    [SerializeField] private RectTransform endOfDaySummary;
    [SerializeField] private TextMeshProUGUI daySummary;
    [SerializeField] private TextMeshProUGUI endOfDayMarker;

    [SerializeField] private RectTransform goalsMenu;
    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private Button goalsButton;

    [SerializeField] private List<Sprite> happinessStates;
    [SerializeField] private List<Sprite> energyStates;

    private void Update()
    {
        timeUI.text = GameManager.instance.gameTime.ReturnTimeString();
        moneyUI.text = $"${GameManager.instance.Money}";
        healthUI.text = $"{GameManager.instance.Health}/100";
        happinessUI.text = $"{GameManager.instance.Happiness}/100";
        hungerUI.text = $"{GameManager.instance.Hunger}/10";
        energyUI.text = $"{GameManager.instance.Energy}/100";
        profileIcon.sprite = GameManager.instance.currentProfile.profileIcon;
        profileName.text = GameManager.instance.currentProfile.profileName;

        if (GameManager.instance.Happiness <= 30)
        {
            happinessIcon.sprite = happinessStates[2];
        }
        else if (GameManager.instance.Happiness >= 31 && GameManager.instance.Happiness <= 70)
        {
            happinessIcon.sprite = happinessStates[1];
        }
        else
        {
            happinessIcon.sprite = happinessStates[0];
        }

        if (GameManager.instance.Energy <= 30)
        {
            energyIcon.sprite = energyStates[2];
        }
        else if (GameManager.instance.Energy >= 31 && GameManager.instance.Energy <= 70)
        {
            energyIcon.sprite = energyStates[1];
        }
        else
        {
            energyIcon.sprite = energyStates[0];
        }
        currentLocation.ShowAvatar(true);
    }

    public void ApplyChanges(Choices choice)
    {
        var currentHunger = GameManager.instance.Hunger;
        var currentEnergy = GameManager.instance.Energy;
        var time = GameManager.instance.gameTime;
        var currLocation = currentOpenLocation == null ? string.Empty : currentOpenLocation.locationName;

        if (currentEnergy + choice.energy < 0 || currentHunger + choice.hunger < 0)
        {
            AlertDialog.instance.ShowAlert("You're too hungry or tired", AlertDialog.AlertLength.Length_Short, AlertDialog.AlertType.CriticalError);
            return;
        }

        AlertDialog.instance.ShowAlert(choice, AlertDialog.AlertLength.Length_Long, AlertDialog.AlertType.Message);

        GoalManager.instance.AddStat(new Stat(choice.statType, time.ReturnDayNumber(), time.ReturnTimePassedForDay(), choice.progressionForStat, currLocation, choice.miscStatParams));
        CloseAllPopups();
        TimerScript.timerController.ResetTime();
        GameManager.instance.Health += choice.healthToAdd;
        GameManager.instance.Happiness += choice.happinessToAdd;
        GameManager.instance.Money += choice.moneyToAdd;
        GameManager.instance.gameTime.AddTime(choice.timeTaken);
        GameManager.instance.Hunger += choice.hunger;
        GameManager.instance.Energy += choice.energy;

        GameManager.instance.StatCheck();
    }

    public void DisplayDecisionPopup(List<Decision> decisionList)
    {
        TimerScript.timerController.Pause(true);
        DisplayPopup(situationPopup);
        var decision = decisionList[UnityEngine.Random.Range(0, decisionList.Count)];
        var children = choicesUI.GetComponentsInChildren<Button>(true);

        var choices = decision.availableChoices;

        decisionStringUI.text = decision.decisionString;
        decisionIcon.sprite = decision.decisionIcon;

        var layout = GetComponentInChildren<FlexibleLayoutGroup>();
        foreach (Transform previousChoice in layout.transform)
        {
            Destroy(previousChoice.gameObject);
        }
        foreach (var choice in decision.availableChoices)
        {
            var button = Instantiate(choiceButton);

            button.GetComponentInChildren<ChoiceButton>().SetUpChoiceButton(choice);

            button.GetComponent<Button>().onClick.AddListener(delegate
            {
                ApplyChanges(choice);
            });

            button.transform.SetParent(layout.transform);
        }
    }

    public void CloseAllPopups()
    {
        infoDisplayHelper.currentOpenLocation = null;
        blackFade.gameObject.SetActive(false);
        goalsButton.gameObject.SetActive(true);
        foreach (var item in GameObject.FindGameObjectsWithTag("PopupUI"))
        {
            item.SetActive(false);
        }
        TimerScript.timerController.Pause(false);
    }

    public void ResetAvatarLocation()
    {
        foreach (var location in locationList)
        {
            location.ShowAvatar(false);
        }
    }

    public void DisplayLocationPopup(Location location)
    {
        var locationInformation = location.locationInformation;
        var gameTime = GameManager.instance.gameTime;
        var timePassedForDay = gameTime.ReturnTimePassedForDay();

        var time = GameManager.instance.gameTime.ReturnTimePassedForDay();
        var currentProfile = GameManager.instance.currentProfile;
        if (time > currentProfile.timeToSleep)
        {
            if (locationInformation.locationName.Equals("Home"))
            {
                EndDay(time);
                return;
            }
            else
            {
                AlertDialog.instance.ShowAlert($"You're out beyond your bed time ({gameTime.CalculateTimeString(currentProfile.timeToSleep)}), return home to sleep", AlertDialog.AlertLength.Length_Long, AlertDialog.AlertType.CriticalError);
                return;
            }
        }

        if (!location.locationInformation.locationName.Equals(currentLocation.locationInformation.locationName))
        {
            ResetAvatarLocation();
            location.ShowAvatar(true);
            currentLocation = location;

            AlertDialog.instance.ShowAlert($"You travelled to {location.locationInformation.locationName}. 30mins Passed -2.5 Energy -0.5 Hunger", AlertDialog.AlertLength.Length_Short, AlertDialog.AlertType.Warning);
            GameManager.instance.gameTime.AddTime(0.5f);
            GameManager.instance.Energy -= 2.5f;
            GameManager.instance.Hunger -= 0.5f;
            GameManager.instance.StatCheck();
            var queryForLocationPopup = currentProfile.situationsForProfile.Where(x => timePassedForDay >= x.startTimeToOccur
            && timePassedForDay <= x.endTimeToOccur
            && x.locationForDecision.locationName.Equals(locationInformation.locationName));

            if (queryForLocationPopup.Any())
            {
                if (UnityEngine.Random.Range(1, 5) == 1)
                {
                    DisplayDecisionPopup(queryForLocationPopup.ToList());
                    return;
                }
            }

            if (locationInformation.situationPopups.Where(x => timePassedForDay >= x.startTimeToOccur
        && timePassedForDay <= x.endTimeToOccur).Any())
            {
                if (UnityEngine.Random.Range(1, 5) == 1)
                {
                    DisplayDecisionPopup(locationInformation.situationPopups);
                    return;
                }
            }
            return;
        }

        //Detect Closing time:
        if (!(time >= locationInformation.openingTime && time <= locationInformation.closingTime) && !locationInformation.is24Hours)
        {
            AlertDialog.instance.ShowAlert($"{locationInformation.locationName} is closed. Opening Hours: {gameTime.CalculateTimeString(locationInformation.openingTime)} - {gameTime.CalculateTimeString(locationInformation.closingTime)}"
                , AlertDialog.AlertLength.Length_Normal, AlertDialog.AlertType.Warning);
            return;
        }

        infoDisplayHelper.currentOpenLocation = locationInformation;
        GoalManager.instance.AddStat(new Stat(Stat.StatType.PlaceVisit, gameTime.ReturnDayNumber(), gameTime.ReturnTimePassedForDay(), 1, locationInformation.locationName, locationInformation.locationName));
        DisplayPopup(placePopup);
        LeanTween.scale(placePopup, new Vector2(1, 1), 0.25f).setFrom(new Vector2(0.5f, 0.5f)).setEase(LeanTweenType.easeInSine);

        placeName.text = locationInformation.locationName;
        backgroundOfPlace.sprite = locationInformation.locationBackground;
        var layout = GetComponentInChildren<FlexibleLayoutGroup>();
        foreach (Transform previousChoice in layout.transform)
        {
            Destroy(previousChoice.gameObject);
        }
        foreach (var choice in locationInformation.thingsToDo)
        {
            GenerateButton(layout, choice);
        }

        if (locationInformation.locationName.Equals("Home"))
        {
            foreach (var choice in currentProfile.homeChoices)
            {
                GenerateButton(layout, choice);
            }

            var button = Instantiate(choiceButton, layout.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = "End the day";

            var newChoice = new Choices
            {
                choiceName = "End the day",
                energy = 25,
                happinessToAdd = 0,
                hunger = -1,
                healthToAdd = 0,
                moneyToAdd = 0,
                timeTaken = 0,
                miscStatParams = string.Empty,
                progressionForStat = 0,
                statType = Stat.StatType.Default
            };
            button.GetComponentInChildren<ChoiceButton>().SetUpChoiceButton(newChoice);
            button.GetComponent<Button>().onClick.AddListener(delegate
            {
                CloseAllPopups();
                EndDay(time);
            });
        }

        if (locationInformation.locationName.Equals("School"))
        {
            foreach (var choice in currentProfile.schoolChoices)
            {
                GenerateButton(layout, choice);
            }
        }

        if (locationInformation.locationName.Equals("Workplace"))
        {
            foreach (var choice in currentProfile.workplaceChoices)
            {
                GenerateButton(layout, choice);
            }
        }
    }

    private void GenerateButton(FlexibleLayoutGroup layout, Choices choice)
    {
        var button = Instantiate(choiceButton);

        button.GetComponentInChildren<ChoiceButton>().SetUpChoiceButton(choice);

        button.GetComponent<Button>().onClick.AddListener(delegate
        {
            ApplyChanges(choice);
        });

        button.transform.SetParent(layout.transform);
    }

    private void EndDay(float time)
    {
        var newChoice = new Choices
        {
            choiceName = "End the day",
            energy = 25,
            happinessToAdd = 0,
            hunger = -2,
            healthToAdd = 0,
            moneyToAdd = 0,
            timeTaken = 0,
            miscStatParams = string.Empty,
            progressionForStat = 0,
            statType = Stat.StatType.Default
        };
        ApplyChanges(newChoice);
        TimerScript.timerController.Pause(true);
        TimerScript.timerController.ResetTime();
        blackFade.gameObject.SetActive(true);
        endOfDaySummary.gameObject.SetActive(true);
        if (time < 24)
        {
            endOfDayMarker.text = $"End of Day {GameManager.instance.gameTime.ReturnDayNumber()}";
        }
        else
        {
            endOfDayMarker.text = $"End of Day {GameManager.instance.gameTime.ReturnDayNumber() - 1}";
        }

        daySummary.text = EndOfDaySummary();
        ResetAvatarLocation();
        currentLocation = locationList.Where(x => x.locationInformation.locationName == "Home").First();
    }

    private string EndOfDaySummary()
    {
        var goalMgr = GoalManager.instance;
        var sb = new StringBuilder();
        sb.AppendLine($"Money Earned: {GameManager.instance.Money - GameManager.instance.oldMoney}");
        sb.AppendLine($"Health Status: {GameManager.instance.Health}/100");
        sb.AppendLine($"Happiness Status: {GameManager.instance.Happiness}/100");

        GameManager.instance.oldMoney = GameManager.instance.Money;
        sb.AppendLine();
        sb.AppendLine("Daily Goal Summary: ");
        var totalGoals = GameManager.instance.currentProfile.goals.Where(x => x.goalType == GoalManager.GoalLength.Daily).Count();
        var completedDaily = goalMgr.completeGoals.Where(x => x.goalType == GoalManager.GoalLength.Daily).ToList();
        var incompletedDaily = goalMgr.incompleteGoals.Where(x => x.goalType == GoalManager.GoalLength.Daily).ToList();

        if (completedDaily.Count != 0)
        {
            sb.AppendLine("Compeleted Goals: ");
            foreach (var completeGoal in completedDaily)
            {
                sb.AppendLine($"- {completeGoal.goalName}");
            }
        }

        if (incompletedDaily.Count != 0)
        {
            sb.AppendLine("Incompleted Goals: ");
            foreach (var incompleteGoal in incompletedDaily)
            {
                sb.AppendLine($"- {incompleteGoal.goalName}");
            }
        }

        sb.AppendLine();
        var percentComplete = ((float)completedDaily.Count / totalGoals) * 100f;
        sb.AppendLine($"Total Complete: {completedDaily.Count}/{totalGoals} ({Math.Truncate(percentComplete)})%");

        if (percentComplete >= 80f)
        {
            sb.AppendLine("You're really on task! +50 Energy +10 Happiness +5 Health");
            GameManager.instance.Energy += 25;
            GameManager.instance.Happiness += 10;
            GameManager.instance.Health += 5;
            sb.AppendLine("Reward: +25 Energy +10 Happiness +5 Health");
        }
        else if (percentComplete <= 79f && percentComplete >= 40f)
        {
            sb.AppendLine("You forgot a few tasks, but good try!");
            GameManager.instance.Energy += 25;
            GameManager.instance.Happiness += 5;
            GameManager.instance.Health += 3;
            sb.AppendLine("Reward: +25 Energy +5 Happiness");
        }
        else
        {
            sb.AppendLine("You didn't manage to complete most of your goals. :(");
            sb.AppendLine("Penalty: -10 Happiness");
            GameManager.instance.Happiness -= 10;
        }

        return sb.ToString();
    }

    public void ContinueToNextDay()
    {
        var time = GameManager.instance.gameTime.ReturnTimePassedForDay();
        var dayToSet = GameManager.instance.gameTime.ReturnDayNumber() - 1;
        var profile = GameManager.instance.currentProfile;
        CloseAllPopups();
        if (time < 24)
        {
            dayToSet += 1;
        }
        GameManager.instance.gameTime.SetTimeRaw((dayToSet * 24) + profile.timeToWake, profile.timeToWake);
        if (dayToSet % 30 == 0)
        {
            SceneManager.LoadScene(2);
        }
    }

    public void DisplayGoal()
    {
        CloseAllPopups();
        goalsMenu.gameObject.SetActive(true);
        blackFade.gameObject.SetActive(true);
        goalText.text = GoalManager.instance.UpdateGoals();
        TimerScript.timerController.Pause(true);
    }

    public void DisplayPopup(RectTransform popup)
    {
        CloseAllPopups();
        popup.gameObject.SetActive(true);
        blackFade.gameObject.SetActive(true);
        goalsButton.gameObject.SetActive(false);
        TimerScript.timerController.Pause(true);
    }
}