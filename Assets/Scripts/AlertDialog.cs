using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class AlertDialog : MonoBehaviour
{
    public enum AlertLength
    {
        Length_Short = 1,
        Length_Normal = 2,
        Length_Long = 4
    }

    public enum AlertType
    {
        CriticalError,
        Warning,
        Message
    }

    public static AlertDialog instance;
    private Image alertBox;
    private readonly Queue<Alert> alertQueue = new Queue<Alert>();
    private RectTransform currentAlert;
    private bool isShowing;
    private Text textUI;

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

    private void Start()
    {
        alertBox = GetComponentInChildren<Image>();
        currentAlert = GetComponent<RectTransform>();
        textUI = GetComponentInChildren<Text>();
        StartCoroutine(GoThroughQueue());
        currentAlert.gameObject.transform.localScale = Vector3.zero;
    }

    public void ShowAlert(string text, AlertLength alertLength, AlertType alertType)
    {
        alertQueue.Enqueue(new Alert(text, alertLength, alertType));
    }
    

    public void DismissAlert(AlertLength alertLength)
    {
        LeanTween.alpha(currentAlert, 0f, 0.5f).setFrom(1f).setEase(LeanTweenType.linear)
            .setDelay((float) alertLength * 0.5f).setOnComplete(HideAlert);
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
                }

                currentAlert.gameObject.transform.localScale = Vector3.one;
                LeanTween.alpha(currentAlert, 1f, 0.25f).setFrom(0f).setEase(LeanTweenType.linear)
                    .setOnComplete(new Action(delegate { DismissAlert(currAlr.alertLength); }));
                yield return new WaitUntil(() => isShowing == false);
            }
        }
    }

    private class Alert
    {
        public readonly AlertLength alertLength;

        public readonly AlertType alertType;
        public readonly string textToDisplay;

        public Alert(string textToDisplay, AlertLength alertLength, AlertType alertType)
        {
            this.textToDisplay = textToDisplay;
            this.alertLength = alertLength;
            this.alertType = alertType;
        }
    }
}