using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Profile[] profiles;
    public bool isInDevelopment = true;
    private Decision[] decisions;
    public static GameManager instance;
    public Profile currentProfile;

    private float _health = 50f;
    private float _happiness = 50f;
    private float _energy = 50f;
    private float _hunger = 10f;

    #region Variable Declaration

    public float Money { get; set; } = 500f;

    public float Energy
    {
        get => _energy;
        set { if (value >= 100) _energy = 100; else if (value < 0) _energy = 0; else _energy = value; }
    }

    public float Hunger
    {
        get => _hunger;
        set { if (value >= 10) _hunger = 10; else if (value < 0) _hunger = 0; else _hunger = value; }
    }

    public float Health
    {
        get => _health;
        set { if (value >= 100) _health = 100; else if (value < 0) _health = 0; else _health = value; }
    }

    public float Happiness
    {
        get => _happiness;
        set { if (value >= 100) _happiness = value; else _happiness = value; }
    }

    public float oldMoney { get; set; }
    public float startMoneyOfMonth { get; set; }

    #endregion Variable Declaration

    public GameTime gameTime = new GameTime();

    public class GameTime
    {
        private float timeInHours = 0f;
        public float timePassedForTheDay = 0f;
        private readonly string[] days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

        public float ReturnTime() => timeInHours;

        public void SetTime(float timeToSet)
        {
            if (timeToSet > 24)
            {
                Debug.LogWarning("Invalid time set: " + timeToSet);
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

        public void ResetTimePassedForDay() => timePassedForTheDay = 0f;

        public float ReturnTimePassedForDay() => timePassedForTheDay;

        public float AddTime(float timeToIncrease)
        {
            timeInHours += timeToIncrease;
            timePassedForTheDay += timeToIncrease;
            return timeInHours;
        }

        public int ReturnDayNumber(float time) => (int)time / 24 + 1;
        public int ReturnDayNumber() => (int)timeInHours / 24 + 1;

        public int ReturnHour(float time) => (int)time % 24;
        public int ReturnHour() => (int)timeInHours % 24;

        public float ReturnHourRaw() => timeInHours % 24;

        public float ReturnMinutes(float time) => (time % 24 % 1) * 60;
        public float ReturnMinutes() => (timeInHours % 24 % 1) * 60;

        public string ReturnTimeString()
        {
            return $"{ReturnHour().ToString().PadLeft(2, '0')}:{ReturnMinutes().ToString().PadLeft(2, '0')} " +
            $"DAY {ReturnDayNumber()} ({ReturnDayOfWeek()})";
        }

        public string CalculateTimeString(float time){
            return $"{ReturnHour(time).ToString().PadLeft(2, '0')}:{ReturnMinutes(time).ToString().PadLeft(2, '0')}";
        }

        public string ReturnDayOfWeek() => days[ReturnDayNumber() % 7];
    }

    private void Awake()
    {
        profiles = Resources.LoadAll<Profile>("Profiles");
        decisions = Resources.LoadAll<Decision>("Decisions");

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        if (instance.currentProfile == null && !isInDevelopment)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            currentProfile = profiles.Where(x => x.profileName == "Student").First();
            SetUpValues();
        }

        
    }
    public Profile ReturnRandomProfile()
    {
        currentProfile = profiles[Random.Range(0, profiles.Length)];
        return currentProfile;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            gameTime.AddTime(23.5f);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            gameTime.AddTime(0.5f);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            gameTime.SetTime(23.5f);
        }
    }

    public Decision ReturnRandomDecision() => decisions[Random.Range(0, decisions.Length)];

    public void SetUpValues(float health, float happiness, float money, float time, float hunger, float energy, float startMoney)
    {
        instance.Energy = energy;
        instance.Health = health;
        instance.Money = money;
        instance.gameTime.SetTime(time);
        instance.Hunger = hunger;
        instance.Happiness = happiness;
        oldMoney = money;
        startMoneyOfMonth = startMoney;
    }

    public void SetUpValues()
    {
        instance.Energy = 50f;
        instance.Health = 50f;
        instance.Money = instance.currentProfile.income;
        instance.gameTime.SetTime(instance.currentProfile.timeToWake);
        instance.Hunger = 10f;
        instance.Happiness = 50f;
        oldMoney = instance.Money;
        startMoneyOfMonth = instance.Money;
    }
}