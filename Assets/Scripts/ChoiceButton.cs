using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    public void SetUpChoiceButton(Choices choices)
    {
        var children = GetComponentsInChildren<TextMeshProUGUI>();

        children[0].text = choices.choiceName;
        children[1].text = HelperFunctions.ReturnSign(choices.healthToAdd) + Mathf.Abs(choices.healthToAdd);
        children[2].text = HelperFunctions.ReturnSign(choices.happinessToAdd) + Mathf.Abs(choices.happinessToAdd);
        children[3].text = HelperFunctions.ReturnSign(choices.timeTaken) + Mathf.Abs(choices.timeTaken) + "h";
        children[4].text = HelperFunctions.ReturnSign(choices.moneyToAdd) + "$" + Mathf.Abs(choices.moneyToAdd);
        children[5].text = HelperFunctions.ReturnSign(choices.hunger) + Mathf.Abs(choices.hunger);
    }

    public void SetUpChoiceButton()
    {
        var children = GetComponentsInChildren<Image>();
        foreach (var child in children) child.gameObject.SetActive(false);
    }
}