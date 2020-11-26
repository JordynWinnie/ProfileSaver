using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

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
    [SerializeField] private GameObject situationPopup;
    [SerializeField] private GameObject placePopup;
    [SerializeField] private TextMeshProUGUI placeName;
    [SerializeField] private Image placeIcon;

    // Update is called once per frame
    private void Update()
    {
        timeUI.text = GameManager.instance.ReturnTimeString();
        moneyUI.text = $"${GameManager.instance.Money}";
        healthUI.text = $"{GameManager.instance.Health}/100";
        happinessUI.text = $"{GameManager.instance.Happiness}/100";
        hungerUI.text = $"{GameManager.instance.Hunger}/10";
        energyUI.text = $"{GameManager.instance.Energy}/100";
        profileIcon.sprite = GameManager.instance.currentProfile.profileIcon;
        profileName.text = GameManager.instance.currentProfile.profileName;

        if (Input.GetKeyDown(KeyCode.A))
        {
            DisplayDecisionPopup();
        }
    }

    public void DisplayDecisionPopup()
    {
        situationPopup.SetActive(true);
        var children = choicesUI.GetComponentsInChildren<Button>(true);
        var decision = GameManager.instance.ReturnRandomDecision();
        var choices = decision.availableChoices;

        decisionStringUI.text = decision.decisionString;
        decisionIcon.sprite = decision.decisionIcon;
        for (int i = 0; i < children.Length; i++)
        {
            children[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < choices.Count; i++)
        {
            var choice = choices[i];
            children[i].gameObject.SetActive(true);
            children[i].onClick.RemoveAllListeners();
            children[i].GetComponentInChildren<TextMeshProUGUI>().text = FormatChoiceText(choice);

            children[i].onClick.AddListener(delegate
            {
                ApplyChanges(choice);
            });
        }
    }

    public void ApplyChanges(Choices choice)
    {
        var currentHunger = GameManager.instance.Hunger;
        var currentEnergy = GameManager.instance.Energy;
        if (currentEnergy + choice.energy < 0 || currentHunger + choice.hunger < 0)
        {
            Debug.LogWarning("Too Hungry or tired");
            return;
        }
        CloseAllPopups();
        GameManager.instance.Health += choice.healthToAdd;
        GameManager.instance.Happiness += choice.happinessToAdd;
        GameManager.instance.Money += choice.moneyToAdd;
        GameManager.instance.AddTime(choice.timeTaken);
        GameManager.instance.Hunger += choice.hunger;
        GameManager.instance.Energy += choice.energy;
    }

    public void CloseAllPopups()
    {
        infoDisplayHelper.currentOpenLocation = null;
        foreach (var item in GameObject.FindGameObjectsWithTag("PopupUI"))
        {
            item.SetActive(false);
        }
    }

    public void DisplayLocationPopup(LocationInformation locationInformation)
    {
        var time = GameManager.instance.ReturnHourRaw();
        var currentProfile = GameManager.instance.currentProfile;
        if (!(time >= locationInformation.openingTime && time <= locationInformation.closingTime) && !locationInformation.is24Hours)
        {
            Debug.LogWarning("Closed");
            return;
        }

        if (time > currentProfile.timeToSleep && !locationInformation.locationName.Equals("Home"))
        {
            Debug.LogWarning("Too Tired");
            return;
        }

        infoDisplayHelper.currentOpenLocation = locationInformation;
        placePopup.SetActive(true);
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

            button.GetComponentInChildren<TextMeshProUGUI>().text = FormatChoiceText(choice);

            button.GetComponent<Button>().onClick.AddListener(delegate
            {
                ApplyChanges(choice);
            });

            button.transform.SetParent(layout.transform);
        }
    }

    private string FormatChoiceText(Choices choice)
    {
        var sb = new StringBuilder();
        sb.AppendLine(choice.choiceName);
        sb.AppendLine($"Takes {choice.timeTaken} hours");
        sb.AppendLine($"{choice.healthToAdd} Health");
        sb.AppendLine($"{choice.energy} Energy");
        sb.AppendLine($"{choice.happinessToAdd} Happiness");
        sb.AppendLine($"{choice.hunger} Food Points");

        sb.AppendLine(choice.moneyToAdd > 0 ? $"${Mathf.Abs(choice.moneyToAdd)}" : $"-${Mathf.Abs(choice.moneyToAdd)}");

        return sb.ToString();
    }
}