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
            DisplayPopup();
        }
    }

    public void DisplayPopup()
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
                ApplyChanges(choice.health, choice.happiness, choice.money, choice.timeTaken, choice.hunger);
            });
        }
    }

    public void ApplyChanges(float healthToChange, float happinessToChange, float moneyToChange, float timeToIncrease, float hungerToIncrease)
    {
        situationPopup.SetActive(false);
        GameManager.instance.Health += healthToChange;
        GameManager.instance.Happiness += happinessToChange;
        GameManager.instance.Money += moneyToChange;
        GameManager.instance.AddTime(timeToIncrease);
        GameManager.instance.Hunger += hungerToIncrease;
    }
}