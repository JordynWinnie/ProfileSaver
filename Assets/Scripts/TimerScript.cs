using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public static TimerScript timerController;
    public float maxTime = 5f;
    public bool isPaused;
    private TextMeshProUGUI secondsTime;
    private float timeLeft;
    private Image timerBar;

    private void Awake()
    {
        if (timerController == null)
            timerController = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        timerBar = GetComponent<Image>();
        timeLeft = maxTime;
        secondsTime = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (timeLeft > 0f)
        {
            if (!isPaused) timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;
            secondsTime.text = Math.Truncate(timeLeft).ToString();
        }
        else
        {
            AlertDialog.instance.ShowAlert("You decided to do nothing. 30mins passed -5 Energy -1 Hunger",
                AlertDialog.AlertLength.Length_Normal, AlertDialog.AlertType.CriticalError);
            GameManager.instance.Energy -= 5;
            GameManager.instance.Hunger -= 1;
            GameManager.instance.gameTime.AddTime(0.5f);
            GameManager.instance.StatCheck();
            timeLeft = maxTime;
        }
    }

    public void Pause(bool pauseState)
    {
        isPaused = pauseState;
    }

    public void ResetTime()
    {
        timeLeft = maxTime;
    }
}