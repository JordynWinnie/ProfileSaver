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
    private Queue<Alert> alertQueue = new Queue<Alert>();
    private bool isShowing = false;

    private class Alert
    {

        public string textToDisplay;
        public AlertLength alertLength;

        public Alert(string textToDisplay, AlertLength alertLength)
        {
            this.textToDisplay = textToDisplay;
            this.alertLength = alertLength;
        }
    }

    public enum AlertLength { Length_Short = 1, Length_Normal = 2, Length_Long = 4 }

    private void Start() {
        currentAlert = GetComponent<RectTransform>();
        textUI = GetComponentInChildren<Text>();
        StartCoroutine(GoThroughQueue());
        currentAlert.gameObject.transform.localScale = Vector3.zero;
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
        alertQueue.Enqueue(new Alert(text, alertLength));
    }

    public void ShowAlert(Choices choices, AlertLength alertLength){
        
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

        alertQueue.Enqueue(new Alert(sb.ToString(), alertLength));
        
    }

    public void DismissAlert(AlertLength alertLength){
        LeanTween.alpha(currentAlert, 0f, 0.5f).setFrom(1f).setEase(LeanTweenType.linear).setDelay((float)alertLength * 0.5f).setOnComplete(HideAlert);
    }

    private void HideAlert(){
        currentAlert.gameObject.transform.localScale = Vector3.zero;
        isShowing = false;
    }

    IEnumerator GoThroughQueue()
    {
        while (true)
        {
            print($"GoThroughQueue: {alertQueue.Count}");
            if (alertQueue.Count == 0)
            {
                yield return null;
            }
            else
            {
                print("Helo");
                var currAlr = alertQueue.Dequeue();
                currentAlert.gameObject.SetActive(true);
                textUI.text = currAlr.textToDisplay;
                isShowing = true;
                currentAlert.gameObject.transform.localScale = Vector3.one;
                LeanTween.alpha(currentAlert, 1f, 1f).setFrom(0f).setEase(LeanTweenType.linear).setOnComplete(new System.Action(delegate { DismissAlert(currAlr.alertLength); }));
                yield return new WaitUntil(() => isShowing == false);
            }
        }
    }
}
