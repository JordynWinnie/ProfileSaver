using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceButton : MonoBehaviour
{
    public void SetUpChoiceButton(Choices choices)
    {
        var children = GetComponentsInChildren<TextMeshProUGUI>();

        children[0].text = choices.choiceName;
        children[1].text = ReturnSign(choices.healthToAdd) + Mathf.Abs(choices.healthToAdd);
        children[2].text = ReturnSign(choices.happinessToAdd) + Mathf.Abs(choices.happinessToAdd);
        children[3].text = ReturnSign(choices.timeTaken) + Mathf.Abs(choices.timeTaken) + "h";
        children[4].text = ReturnSign(choices.energy) + Mathf.Abs(choices.energy);
        children[5].text = ReturnSign(choices.moneyToAdd) + "$" + Mathf.Abs(choices.moneyToAdd);
        children[6].text = ReturnSign(choices.hunger) + Mathf.Abs(choices.hunger);
    }

    private string ReturnSign(float number)
    {
        if (number == 0)
        {
            return string.Empty;
        }

        if (number > 0)
        {
            return "+";
        }
        else
        {
            return "-";
        }
    }
}