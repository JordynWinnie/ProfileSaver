using System;

[Serializable]
public class Stat
{
    public enum StatType
    {
        Default,
        StudyTime,
        WorkTime,
        PlaceVisit,
        MealTaken,
        ItemPurchased,
        MoneyToSave
    }

    public StatType statType;
    public float dayOfAction;
    public float timeOfAction;
    public float actionCount;
    public string placeVisited;
    public string miscStatParams;

    public Stat(StatType statType, float dayOfAction, float timeOfAction, float actionCount, string placeVisited,
        string miscStatParams = null)
    {
        this.timeOfAction = timeOfAction;
        this.dayOfAction = dayOfAction;
        this.actionCount = actionCount;
        this.placeVisited = placeVisited;
        this.miscStatParams = miscStatParams;
        this.statType = statType;
    }
}