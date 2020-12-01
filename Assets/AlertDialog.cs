using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class AlertDialog : MonoBehaviour
{
    public static AlertDialog instance;
    private RectTransform currentAlert;
    private Text textUI;

    public enum AlertLength { Length_Short = 1, Length_Normal = 2, Length_Long = 4 }

    private void Start() {
        currentAlert = GetComponent<RectTransform>();
        textUI = GetComponentInChildren<Text>();
        currentAlert.gameObject.SetActive(false);
    }
    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }else
        {
            Destroy(instance.gameObject);
        }
    }

    public void ShowAlert(string text, AlertLength alertLength){
        currentAlert.gameObject.SetActive(true);
        textUI.text = text;
        LeanTween.alpha(currentAlert, 1f, 1f).setFrom(0f).setEase(LeanTweenType.linear).setOnComplete(new System.Action(delegate {DismissAlert(alertLength);}));
    }

    public void ShowAlert(Choices choices, AlertLength alertLength){
        currentAlert.gameObject.SetActive(true);
        var sb = new StringBuilder();
        if (choices.timeTaken != 0)
        {
            sb.Append($"{choices.timeTaken}h Passed ");
        }
        if (choices.energy != 0)
        {
            sb.Append($"{HelperFunctions.ReturnSign(choices.energy)}{Mathf.Abs(choices.energy)} energy ");
        }
        if (choices.healthToAdd != 0)
        {
           sb.Append($"{HelperFunctions.ReturnSign(choices.healthToAdd)}{Mathf.Abs(choices.healthToAdd)} health ");
        }
        if (choices.happinessToAdd != 0)
        {
            sb.Append($"{HelperFunctions.ReturnSign(choices.happinessToAdd)}{Mathf.Abs(choices.happinessToAdd)} happiness ");
        }
        if (choices.hunger != 0)
        {
            sb.Append($"{HelperFunctions.ReturnSign(choices.hunger)}{Mathf.Abs(choices.hunger)} hunger ");
        }
        if (choices.moneyToAdd != 0)
        {
            sb.Append($"{HelperFunctions.ReturnSign(choices.moneyToAdd)}${Mathf.Abs(choices.moneyToAdd)} ");
        }

        textUI.text = sb.ToString();
        LeanTween.alpha(currentAlert, 1f, 1f).setFrom(0f).setEase(LeanTweenType.linear).setOnComplete(new System.Action(delegate {DismissAlert(alertLength);}));
    }

    public void DismissAlert(AlertLength alertLength){
        LeanTween.alpha(currentAlert, 0f, 0.5f).setFrom(1f).setEase(LeanTweenType.linear).setDelay((float)alertLength * 0.5f).setOnComplete(HideAlert);
    }

    private void HideAlert(){
        currentAlert.gameObject.SetActive(false);
    }
}
