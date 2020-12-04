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
    private Image alertBox;
    private Queue<Alert> alertQueue = new Queue<Alert>();
    private bool isShowing = false;

    private class Alert
    {
        public string textToDisplay;
        public AlertLength alertLength;

        public AlertType alertType;

        public Alert(string textToDisplay, AlertLength alertLength, AlertType alertType)
        {
            this.textToDisplay = textToDisplay;
            this.alertLength = alertLength;
            this.alertType = alertType;
        }
    }

    public enum AlertLength { Length_Short = 1, Length_Normal = 2, Length_Long = 4 }

    public enum AlertType { CriticalError, Warning, Message }

    private void Start()
    {
        alertBox = GetComponentInChildren<Image>();
        currentAlert = GetComponent<RectTransform>();
        textUI = GetComponentInChildren<Text>();
        StartCoroutine(GoThroughQueue());
        currentAlert.gameObject.transform.localScale = Vector3.zero;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
        }
    }

    public void ShowAlert(string text, AlertLength alertLength, AlertType alertType)
    {
        alertQueue.Enqueue(new Alert(text, alertLength, alertType));
    }

    public void ShowAlert(Choices choices, AlertLength alertLength, AlertType alertType)
    {
        var sb = new StringBuilder();
        sb.Append($"{choices.choiceName}: ");
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

        alertQueue.Enqueue(new Alert(sb.ToString(), alertLength, alertType));
    }

    public void DismissAlert(AlertLength alertLength)
    {
        LeanTween.alpha(currentAlert, 0f, 0.5f).setFrom(1f).setEase(LeanTweenType.linear).setDelay((float)alertLength * 0.5f).setOnComplete(HideAlert);
    }

    private void HideAlert()
    {
        currentAlert.gameObject.transform.localScale = Vector3.zero;
        isShowing = false;
    }

    private IEnumerator GoThroughQueue()
    {
        while (true)
        {
            if (alertQueue.Count == 0)
            {
                yield return null;
            }
            else
            {
                var currAlr = alertQueue.Dequeue();
                currentAlert.gameObject.SetActive(true);
                textUI.text = currAlr.textToDisplay;
                isShowing = true;

                switch (currAlr.alertType)
                {
                    case AlertType.CriticalError:
                        alertBox.color = Color.red;
                        break;

                    case AlertType.Warning:
                        alertBox.color = new Color(0.8f, 0.39f, 0.12f);
                        break;

                    case AlertType.Message:
                        alertBox.color = new Color(0.07f, 0.63f, 0.07f);
                        break;

                    default:
                        break;
                }

                currentAlert.gameObject.transform.localScale = Vector3.one;
                LeanTween.alpha(currentAlert, 1f, 0.25f).setFrom(0f).setEase(LeanTweenType.linear).setOnComplete(new System.Action(delegate { DismissAlert(currAlr.alertLength); }));
                yield return new WaitUntil(() => isShowing == false);
            }
        }
    }
}