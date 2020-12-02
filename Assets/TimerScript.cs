using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TimerScript : MonoBehaviour
{
    Image timerBar;
    public float maxTime = 60f;
    float timeLeft = 0f;
    TextMeshProUGUI secondsTime;

    // Start is called before the first frame update
    void Start()
    {
        timerBar = GetComponent<Image>();
        timeLeft = maxTime;
        secondsTime = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;
            secondsTime.text = Math.Truncate(timeLeft).ToString();
        }
        else
        {
            timeLeft = 60f;
        }
    }
}
