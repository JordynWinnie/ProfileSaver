using UnityEngine;
using TMPro;

public class ChoiceButton : MonoBehaviour
{
    public void SetUpChoiceButton(Choices choices)
    {
        var children = GetComponentsInChildren<TextMeshProUGUI>();

        children[0].text = choices.choiceName;
        children[1].text = HelperFunctions.ReturnSign(choices.healthToAdd) + Mathf.Abs(choices.healthToAdd);
        children[2].text = HelperFunctions.ReturnSign(choices.happinessToAdd) + Mathf.Abs(choices.happinessToAdd);
        children[3].text = HelperFunctions.ReturnSign(choices.timeTaken) + Mathf.Abs(choices.timeTaken) + "h";
        children[4].text = HelperFunctions.ReturnSign(choices.energy) + Mathf.Abs(choices.energy);
        children[5].text = HelperFunctions.ReturnSign(choices.moneyToAdd) + "$" + Mathf.Abs(choices.moneyToAdd);
        children[6].text = HelperFunctions.ReturnSign(choices.hunger) + Mathf.Abs(choices.hunger);
    }
}