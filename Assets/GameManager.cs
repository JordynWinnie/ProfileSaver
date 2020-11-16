using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Profile> profiles;
    public static GameManager instance;
    public Profile currentProfile;

    private void Awake()
    {
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
        currentProfile = profiles[Random.Range(0, profiles.Count)];
        print(currentProfile.profileName);
        return currentProfile;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}