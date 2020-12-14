using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isGameLoad = false;
    public static Profile profileToLoad;
    public static bool isContinueMonth = false;

    public static GameManager instance;

    public Profile currentProfile;

    public bool isInDevelopment = false;

    public Location currentLocation;
    [SerializeField]
    public List<Location> locationsList;

    public List<Stat> statList;

    private float _happiness = 50f;

    private float _health = 50f;
    private float _hunger = 10f;

    public GameTime gameTime = new GameTime();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (isInDevelopment)
        {
            var profiles = Resources.LoadAll<Profile>("Profiles");
            currentProfile = profiles.First(x => x.profileName == "Student");
            print("DeveloperMode");
            SetUpValues();
            return;
        }

        if (isGameLoad)
        {
            print("ValueOfIsContinue: " + isContinueMonth);
            print("GameLoad");
            
            if (isContinueMonth)
            {
                print("IsContinueMonth");
                
                SetUpValues(SaveSystem.LoadData());
                
                //AlertDialog.instance.ShowAlert($"Monthly Income: +${instance.currentProfile.income}",
                    //AlertDialog.AlertLength.Length_Normal, AlertDialog.AlertType.Message);
                return;
            }
            
            SetUpValues(SaveSystem.LoadData());
        }
        else
        {
            print("Game Start");
            currentProfile = profileToLoad;
            profileToLoad = null;
            SetUpValues();
        }
    }


    private void Update()
    {
        print("GameTime: " + gameTime.timeInHours);
        if (Input.GetKeyDown(KeyCode.Q)) gameTime.AddTime(696f);

        if (Input.GetKeyDown(KeyCode.W)) gameTime.AddTime(0.5f);

        if (Input.GetKeyDown(KeyCode.T)) gameTime.SetTime(23.5f);

        if (Input.GetKeyDown(KeyCode.Y))
            foreach (var stat in statList)
                print(stat.placeVisited);

        if (Input.GetKeyDown(KeyCode.S)) SaveSystem.SaveData(instance);

        if (currentProfile == null) return;
        
        foreach (var location in locationsList)
        {
            print(location.locationInformation.locationName);
            location.avatarLocation.sprite = currentProfile.profileIcon;
            location.ShowAvatar(false);
        }
        currentLocation.ShowAvatar(true);
    }

    public void SetUpValues(SaveData saveData)
    {
        print("LocationSaved: "  + saveData.currentLocation);
        var profiles = Resources.LoadAll<Profile>("Profiles");
        instance.Health = saveData.health;
        instance.Money = saveData.money;
        print(saveData.timeInHours);
        instance.gameTime.SetTimeRaw(saveData.timeInHours, saveData.timePassedForDay);
        instance.Hunger = saveData.hunger;
        instance.Happiness = saveData.happiness;
        oldMoney = saveData.oldMoney;
        startMoneyOfMonth = saveData.startMoneyOfMonth;
        GoalManager.instance.trackedStatistics = saveData.statList;
        currentProfile = profiles.First(x => x.profileName == saveData.currentProfile);
        var location1 = locationsList.FirstOrDefault(x => x.locationInformation.locationName == saveData.currentLocation);
        print("LocationLoaded: " + location1.locationInformation.locationName);
        instance.currentLocation = location1;
    }

    public void SetUpValues()
    {

        instance.Health = 75f;
        instance.Money = instance.currentProfile.income;
        instance.gameTime.SetTime(instance.currentProfile.timeToWake);
        instance.Hunger = 10f;
        instance.Happiness = 50f;
        oldMoney = instance.Money;
        startMoneyOfMonth = instance.Money;
        instance.currentLocation = locationsList.FirstOrDefault(x => x.locationInformation.locationName == "Home");
    }

    public void StatCheck()
    {
        if (instance.Hunger <= 1)
        {
            AlertDialog.instance.ShowAlert("You got really hungry. -5 Happiness -2 Health",
                AlertDialog.AlertLength.Length_Normal, AlertDialog.AlertType.CriticalError);
            instance.Happiness -= 5;
            instance.Health -= 2;
        }

        if (instance.Happiness <= 15)
        {
            AlertDialog.instance.ShowAlert("You are very sad. -2 Health", AlertDialog.AlertLength.Length_Normal,
                AlertDialog.AlertType.CriticalError);
            instance.Health -= 2;
        }

        if (instance.Health <= 10)
            AlertDialog.instance.ShowAlert("Warning, your health is in critical condition",
                AlertDialog.AlertLength.Length_Long, AlertDialog.AlertType.Warning);
        else if (instance.Health <= 25)
            AlertDialog.instance.ShowAlert("Warning, your health is in bad condition",
                AlertDialog.AlertLength.Length_Long, AlertDialog.AlertType.CriticalError);

        SaveSystem.SaveData(instance);
        if (Health <= 0)
        {
            SceneManager.LoadScene(3);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        instance.currentProfile = null;
        isGameLoad = false;
        isContinueMonth = false;
        SceneManager.LoadScene(0);
    }


    public class GameTime
    {
        private readonly string[] days = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"};
        public float timeInHours;
        public float timePassedForTheDay;

        public float ReturnTime()
        {
            return timeInHours;
        }

        public void SetTime(float timeToSet)
        {
            if (timeToSet > 24)
            {
                return;
            }

            timeInHours = (ReturnDayNumber() - 1) * 24;
            timePassedForTheDay = 0f;
            timeInHours += timeToSet;
            timePassedForTheDay += timeToSet;
        }

        public void SetTimeRaw(float timeToSetInHours, float timePassedForDay)
        {
            timeInHours = timeToSetInHours;
            timePassedForTheDay = timePassedForDay;
        }

        public void ResetTimePassedForDay()
        {
            timePassedForTheDay = 0f;
        }

        public float ReturnTimePassedForDay()
        {
            return timePassedForTheDay;
        }

        public float AddTime(float timeToIncrease)
        {
            timeInHours += timeToIncrease;
            timePassedForTheDay += timeToIncrease;
            return timeInHours;
        }

        public int ReturnDayNumber(float time)
        {
            return (int) time / 24 + 1;
        }

        public int ReturnDayNumber()
        {
            return (int) timeInHours / 24 + 1;
        }

        public int ReturnHour(float time)
        {
            return (int) time % 24;
        }

        public int ReturnHour()
        {
            return (int) timeInHours % 24;
        }

        public float ReturnHourRaw()
        {
            return timeInHours % 24;
        }

        public float ReturnMinutes(float time)
        {
            return time % 24 % 1 * 60;
        }

        public float ReturnMinutes()
        {
            return timeInHours % 24 % 1 * 60;
        }

        public string ReturnTimeString()
        {
            return $"{ReturnHour().ToString().PadLeft(2, '0')}:{ReturnMinutes().ToString().PadLeft(2, '0')} " +
                   $"DAY {ReturnDayNumber()} ({ReturnDayOfWeek()})";
        }

        public string CalculateTimeString(float time)
        {
            return $"{ReturnHour(time).ToString().PadLeft(2, '0')}:{ReturnMinutes(time).ToString().PadLeft(2, '0')}";
        }

        public string ReturnDayOfWeek()
        {
            return days[ReturnDayNumber() % 7];
        }
    }

    #region Variable Declaration

    public float Money { get; set; } = 500f;
    
    public float Hunger
    {
        get => _hunger;
        set
        {
            if (value >= 10) _hunger = 10;
            else if (value <= 0) _hunger = 0;
            else _hunger = value;
        }
    }

    public float Health
    {
        get => _health;
        set
        {
            if (value >= 100) _health = 100;
            else if (value < 0) _health = 0;
            else _health = value;
        }
    }

    public float Happiness
    {
        get => _happiness;
        set
        {
            if (value >= 100) _happiness = value;
            else if (value < 0) _happiness = 0;
            else _happiness = value;
        }
    }

    public float oldMoney { get; set; }
    public float startMoneyOfMonth { get; set; }

    #endregion Variable Declaration
}