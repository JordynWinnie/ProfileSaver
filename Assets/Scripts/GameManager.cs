using System.Collections;
using System.Collections.Generic;
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

    public float Health
    {
        get => _health;
        set
        {
            if (value <= 100)
            {
                _health = value;
            }
        }
    }

    public float Happiness
    {
        get => _happiness;
        set
        {
            if (value <= 100)
            {
                _happiness = value;
            }
        }
    }

    public float Money { get; set; } = 1000f;

    private float timeInHours = 0f;

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
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        if (currentProfile == null && !isInDevelopment)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            currentProfile = profiles.Where(x => x.profileName == "Student").First();
        }
    }

    public Profile ReturnRandomProfile()
    {
        currentProfile = profiles[Random.Range(0, profiles.Length)];
        return currentProfile;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddTime(24f);
            print(ReturnTimeString());
        }
    }

    private void Start()
    {
    }

    public float ReturnTime()
    {
        return timeInHours;
    }

    public float AddTime(float timeToIncrease)
    {
        timeInHours += timeToIncrease;
        return timeInHours;
    }

    public int ReturnDayNumber()
    {
        return (int)timeInHours / 24 + 1;
    }

    public int ReturnHour()
    {
        return (int)timeInHours % 24;
    }

    public float ReturnMinutes()
    {
        return (timeInHours % 24 % 1) * 60;
    }

    public string ReturnTimeString()
    {
        var isWeekend = ReturnDayNumber() % 7 == 0 || (ReturnDayNumber() + 1) % 7 == 0;
        if (isWeekend)
        {
            return $"{ReturnHour().ToString().PadLeft(2, '0')}:{ReturnMinutes().ToString().PadLeft(2, '0')} " +
            $"DAY {ReturnDayNumber()} (Weekend)";
        }
        else
        {
            return $"{ReturnHour().ToString().PadLeft(2, '0')}:{ReturnMinutes().ToString().PadLeft(2, '0')} " +
            $"DAY {ReturnDayNumber()} (Weekday)";
        }
    }

    public Decision ReturnRandomDecision()
    {
        return decisions[Random.Range(0, decisions.Length)];
    }
}