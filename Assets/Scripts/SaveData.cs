using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<Stat> statList;
    public float startMoneyOfMonth;
    public float money;
    public float hunger;
    public float health;
    public float oldMoney;
    public float happiness;
    public string currentProfile;
    public float timePassedForDay;
    public float timeInHours;
    public string currentLocation;
    public int meatAmount;
    public int vegetableAmount;
    public int fishAmount;
    public int fruitAmount;

    public SaveData(GameManager gameManager)
    {
        statList = gameManager.statList;
        startMoneyOfMonth = gameManager.startMoneyOfMonth;
        money = gameManager.Money;
        hunger = gameManager.Hunger;
        health = gameManager.Health;
        oldMoney = gameManager.oldMoney;
        happiness = gameManager.Happiness;
        currentProfile = gameManager.currentProfile.profileName;
        timePassedForDay = gameManager.gameTime.timePassedForTheDay;
        timeInHours = gameManager.gameTime.timeInHours;
        happiness = gameManager.Happiness;
        currentLocation = gameManager.currentLocation.locationInformation.locationName;
        meatAmount = gameManager.MeatAmt;
        vegetableAmount = gameManager.VegetableAmt;
        fishAmount = gameManager.FishAmt;
        fruitAmount = gameManager.FruitsAmt;
    }
}