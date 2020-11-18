using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Profile[] profiles;
    private Decision[] decisions;
    public static GameManager instance;
    public Profile currentProfile;
    
    private float _health = 10f;
    private float _happiness = 10f;
    private float _money = 1000f;

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

    public float Money { get => _money; set => _money = value; }

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
            AddTime(0.5f);
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
        if (ReturnHour() < 12)
        {
            return $"{ReturnHour().ToString().PadLeft(2, '0')}:{ReturnMinutes().ToString().PadLeft(2, '0')} AM DAY {ReturnDayNumber()}";
        }
        else
        {
            return $"{ReturnHour().ToString().PadLeft(2, '0')}:{ReturnMinutes().ToString().PadLeft(2, '0')} PM DAY {ReturnDayNumber()}";
        }
    }

    public Decision ReturnRandomDecision()
    {
        return decisions[Random.Range(0, decisions.Length)];
    }
}