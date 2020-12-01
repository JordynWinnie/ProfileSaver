using System.Collections;
using System.Collections.Generic;
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
        //ShowAlert("Hello World", AlertLength.Length_Short);
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
        textUI.text = text;
        LeanTween.alpha(currentAlert, 1f, 1f).setFrom(0f).setEase(LeanTweenType.linear).setOnComplete(new System.Action(delegate {DismissAlert(alertLength);}));
    }

    public void DismissAlert(AlertLength alertLength){
        LeanTween.alpha(currentAlert, 0f, 0.5f).setFrom(1f).setEase(LeanTweenType.linear).setDelay((float)alertLength * 0.5f);
    }
}
