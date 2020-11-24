using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInformation : MonoBehaviour
{
    public static DisplayInformation infoDisplayHelper;

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
        timeUI.text = "Time: " + GameManager.instance.ReturnTimeString();
        moneyUI.text = "Money: $" + GameManager.instance.Money.ToString();
        healthUI.text = "Health: " + GameManager.instance.Health.ToString();
        happinessUI.text = "Happiness: " + GameManager.instance.Happiness.ToString();
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
            children[i].GetComponentInChildren<TextMeshProUGUI>().text
                = $"{choice.choiceName}\n{choice.health} health\n{choice.happiness} happiness\n${choice.money}";

            children[i].onClick.AddListener(delegate
            {
                ApplyChanges(choice.health, choice.happiness, choice.money, choice.timeTaken, choice.hunger, choice.energy);
            });
        }
    }

    public void ApplyChanges(float healthToChange, float happinessToChange, float moneyToChange, float timeToIncrease, float hungerToIncrease, float energyToIncrease)
    {
        situationPopup.SetActive(false);
        GameManager.instance.Health += healthToChange;
        GameManager.instance.Happiness += happinessToChange;
        GameManager.instance.Money += moneyToChange;
        GameManager.instance.AddTime(timeToIncrease);
        GameManager.instance.Hunger += hungerToIncrease;
        GameManager.instance.Energy += energyToIncrease;
    }

    public void CloseAllPopups()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("PopupUI"))
        {
            item.SetActive(false);
        }
    }

    public void DisplayLocationPopup(LocationInformation locationInformation)
    {
        placePopup.SetActive(true);
        print(locationInformation.locationName);
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

            button.GetComponentInChildren<TextMeshProUGUI>().text = $"{choice.choiceName}\n{choice.health} health\n{choice.happiness} happiness\n${choice.money}";

            button.GetComponent<Button>().onClick.AddListener(delegate
            {
                ApplyChanges(choice.health, choice.happiness, choice.money, choice.timeTaken, choice.hunger, choice.energy);
            });

            button.transform.parent = layout.transform;
        }
    }
}