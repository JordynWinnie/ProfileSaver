using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Profile[] profiles;
    public static GameManager instance;
    public Profile currentProfile;

    private void Awake()
    {
        profiles = Resources.LoadAll<Profile>("Profiles");

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
        print(currentProfile.profileName);
        return currentProfile;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void Start()
    {
        foreach (var profile in profiles)
        {
            print(profile.profileName);
        }
    }
}