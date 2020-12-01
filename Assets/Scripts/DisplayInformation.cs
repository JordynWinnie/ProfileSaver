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
    [SerializeField] private Image placeIcon;
    [SerializeField] private RectTransform blackFade;

    [SerializeField] private RectTransform endOfDaySummary;
    [SerializeField] private TextMeshProUGUI daySummary;
    [SerializeField] private TextMeshProUGUI endOfDayMarker;

    [SerializeField] private RectTransform goalsMenu;
    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private Button goalsButton;

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
    }

    public void DisplayDecisionPopup(List<Decision> decisionList)
    {
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

    public void ApplyChanges(Choices choice)
    {
        var currentHunger = GameManager.instance.Hunger;
        var currentEnergy = GameManager.instance.Energy;
        var time = GameManager.instance.gameTime;
        var currLocation = currentOpenLocation == null ? string.Empty : currentOpenLocation.locationName;
        

        if (currentEnergy + choice.energy < 0 || currentHunger + choice.hunger < 0)
        {
            AlertDialog.instance.ShowAlert("You're too hungry or tired", AlertDialog.AlertLength.Length_Normal);
            return;
        }

        AlertDialog.instance.ShowAlert(choice, AlertDialog.AlertLength.Length_Long);

        GoalManager.instance.AddStat(new Stat(choice.statType, time.ReturnDayNumber(), time.ReturnTimePassedForDay(), choice.progressionForStat, currLocation, choice.miscStatParams));
        CloseAllPopups();
        GameManager.instance.Health += choice.healthToAdd;
        GameManager.instance.Happiness += choice.happinessToAdd;
        GameManager.instance.Money += choice.moneyToAdd;
        GameManager.instance.gameTime.AddTime(choice.timeTaken);
        GameManager.instance.Hunger += choice.hunger;
        GameManager.instance.Energy += choice.energy;
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
    }

    public void DisplayLocationPopup(LocationInformation locationInformation)
    {
        var gameTime = GameManager.instance.gameTime;
        var timePassedForDay = gameTime.ReturnTimePassedForDay();
        if (locationInformation.situationPopups.Where(x => timePassedForDay >= x.startTimeToOccur
        && timePassedForDay <= x.endTimeToOccur).Any())
        {
            if (Random.Range(1, 5) == 1)
            {
                DisplayDecisionPopup(locationInformation.situationPopups);
                return;
            }
        }

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
                AlertDialog.instance.ShowAlert($"You're out beyond your bed time ({gameTime.CalculateTimeString(currentProfile.timeToSleep)}), return home to sleep", AlertDialog.AlertLength.Length_Long);
                return;
            }
        }

        //Detect Closing time:
        if (!(time >= locationInformation.openingTime && time <= locationInformation.closingTime) && !locationInformation.is24Hours)
        {
            AlertDialog.instance.ShowAlert($"{locationInformation.locationName} is closed. Opening Hours: {gameTime.CalculateTimeString(locationInformation.openingTime)} - {gameTime.CalculateTimeString(locationInformation.closingTime)}", AlertDialog.AlertLength.Length_Long);
            return;
        }

        infoDisplayHelper.currentOpenLocation = locationInformation;
        GoalManager.instance.AddStat(new Stat(Stat.StatType.PlaceVisit, gameTime.ReturnDayNumber(), gameTime.ReturnTimePassedForDay(), 1, locationInformation.locationName, locationInformation.locationName));
        DisplayPopup(placePopup);
        LeanTween.scale(placePopup, new Vector2(1, 1), 0.25f).setFrom(new Vector2(0.5f, 0.5f)).setEase(LeanTweenType.easeInSine);
        placeIcon.sprite = locationInformation.locationSprite;
        placeName.text = locationInformation.locationName;
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
    }

    private void EndDay(float time)
    {
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
    }

    public void DisplayPopup(RectTransform popup)
    {
        CloseAllPopups();
        popup.gameObject.SetActive(true);
        blackFade.gameObject.SetActive(true);
        goalsButton.gameObject.SetActive(false);
    }
}