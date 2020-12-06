using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<Stat> statList;
    public float startMoneyOfMonth;
    public float money;
    public float energy;
    public float hunger;
    public float health;
    public float oldMoney;
    public float happiness;
    public string currentProfile;
    public float timePassedForDay;
    public float timeInHours;
    public string currentLocation;

    public SaveData(GameManager gameManager)
    {
        statList = gameManager.statList;
        startMoneyOfMonth = gameManager.startMoneyOfMonth;
        money = gameManager.Money;
        energy = gameManager.Energy;
        hunger = gameManager.Hunger;
        health = gameManager.Health;
        oldMoney = gameManager.oldMoney;
        happiness = gameManager.Happiness;
        currentProfile = gameManager.currentProfile.profileName;
        timePassedForDay = gameManager.gameTime.timePassedForTheDay;
        timeInHours = gameManager.gameTime.timeInHours;
        happiness = gameManager.Happiness;
        currentLocation = gameManager.currentLocation.locationInformation.locationName;
    }
}