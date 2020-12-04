public class Stat
{
    public enum StatType { Default, StudyTime, WorkTime, PlaceVisit, MealTaken, ItemPurchased, MoneyToSave }

    public StatType statType { get; set; }
    public float dayOfAction { get; set; }
    public float timeOfAction { get; set; }
    public float actionCount { get; set; }
    public string placeVisited { get; set; }
    public string miscStatParams { get; set; }

    public Stat(StatType statType, float dayOfAction, float timeOfAction, float actionCount, string placeVisited, string miscStatParams = null)
    {
        this.timeOfAction = timeOfAction;
        this.dayOfAction = dayOfAction;
        this.actionCount = actionCount;
        this.placeVisited = placeVisited;
        this.miscStatParams = miscStatParams;
        this.statType = statType;
    }
}