using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TimerScript : MonoBehaviour
{
    private Image timerBar;
    public float maxTime = 5f;
    private float timeLeft = 0f;
    private TextMeshProUGUI secondsTime;
    public bool isPaused = false;

    public static TimerScript timerController = null;

    private void Awake()
    {
        if (timerController == null)
        {
            timerController = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
            if (!isPaused)
            {
                timeLeft -= Time.deltaTime;
            }
            timerBar.fillAmount = timeLeft / maxTime;
            secondsTime.text = Math.Truncate(timeLeft).ToString();
        }
        else
        {
            AlertDialog.instance.ShowAlert("You decided to do nothing. 30mins passed -5 Energy -1 Hunger", AlertDialog.AlertLength.Length_Normal, AlertDialog.AlertType.CriticalError);
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