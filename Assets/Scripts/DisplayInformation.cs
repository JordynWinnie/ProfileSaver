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

    // Update is called once per frame
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
        var decision = decisionList[Random.Range(0, decisionList.Count)];
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

            AlertDialog.instance.ShowAlert($"You travelled to {location.locationInformation.locationName}. 30mins Passed -5 Energy -1 Hunger", AlertDialog.AlertLength.Length_Short, AlertDialog.AlertType.Warning);
            GameManager.instance.gameTime.AddTime(0.5f);
            GameManager.instance.Energy -= 5;
            GameManager.instance.Hunger -= 1;
            GameManager.instance.StatCheck();
            var queryForLocationPopup = currentProfile.situationsForProfile.Where(x => timePassedForDay >= x.startTimeToOccur
            && timePassedForDay <= x.endTimeToOccur
            && x.locationForDecision.locationName.Equals(locationInformation.locationName));

            if (queryForLocationPopup.Any())
            {
                if (Random.Range(1, 5) == 1)
                {
                    DisplayDecisionPopup(queryForLocationPopup.ToList());
                    return;
                }
            }

            if (locationInformation.situationPopups.Where(x => timePassedForDay >= x.startTimeToOccur
        && timePassedForDay <= x.endTimeToOccur).Any())
            {
                if (Random.Range(1, 5) == 1)
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
            var button = Instantiate(choiceButton);

            button.GetComponentInChildren<ChoiceButton>().SetUpChoiceButton(choice);

            button.GetComponent<Button>().onClick.AddListener(delegate
            {
                ApplyChanges(choice);
            });

            button.transform.SetParent(layout.transform);
        }

        if (locationInformation.locationName.Equals("Home"))
        {
            var button = Instantiate(choiceButton);

            button.GetComponentInChildren<TextMeshProUGUI>().text = "End the day";

            button.GetComponent<Button>().onClick.AddListener(delegate
            {
                CloseAllPopups();
                EndDay(time);
            });

            button.transform.SetParent(layout.transform);
        }

        if (locationInformation.locationName.Equals("School"))
        {
            foreach (var thingsToDo in currentProfile.schoolChoices)
            {
                var button = Instantiate(choiceButton, layout.transform);

                button.GetComponentInChildren<ChoiceButton>().SetUpChoiceButton(thingsToDo);

                button.GetComponent<Button>().onClick.AddListener(delegate
                {
                    ApplyChanges(thingsToDo);
                });
            }
        }

        if (locationInformation.locationName.Equals("Workplace"))
        {
            print("Workplace Called");
            foreach (var thingsToDo in currentProfile.workplaceChoices)
            {
                var button = Instantiate(choiceButton, layout.transform);

                button.GetComponentInChildren<ChoiceButton>().SetUpChoiceButton(thingsToDo);

                button.GetComponent<Button>().onClick.AddListener(delegate
                {
                    ApplyChanges(thingsToDo);
                });
            }
        }
    }

    private void EndDay(float time)
    {
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
        var sb = new StringBuilder();
        sb.AppendLine($"Money Earned: {GameManager.instance.Money - GameManager.instance.oldMoney}");
        sb.AppendLine($"Health Status: {GameManager.instance.Health}/100");
        sb.AppendLine($"Happiness Status: {GameManager.instance.Happiness}/100");

        GameManager.instance.oldMoney = GameManager.instance.Money;

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
        goalText.text = GoalManager.instance.PrintGoals();
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