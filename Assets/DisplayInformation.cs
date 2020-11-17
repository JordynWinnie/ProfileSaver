using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInformation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeUI;
    [SerializeField] private TextMeshProUGUI moneyUI;
    [SerializeField] private TextMeshProUGUI healthUI;
    [SerializeField] private TextMeshProUGUI happinessUI;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        timeUI.text = "Time: " + GameManager.instance.ReturnTimeString();
        moneyUI.text = "Money: $" + GameManager.instance.Money.ToString();
        healthUI.text = "Health: " + GameManager.instance.Health.ToString();
        happinessUI.text = "Happiness: " + GameManager.instance.Happiness.ToString();
    }
}