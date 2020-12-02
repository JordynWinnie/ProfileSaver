using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TimerScript : MonoBehaviour
{
    private Image timerBar;
    public float maxTime = 60f;
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
            Destroy(this.gameObject);
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
            AlertDialog.instance.ShowAlert("You decided to do nothing. -5 Energy -1 Hunger", AlertDialog.AlertLength.Length_Normal);
            GameManager.instance.Energy -= 5;
            GameManager.instance.Hunger -= 1;
            timeLeft = 60f;
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