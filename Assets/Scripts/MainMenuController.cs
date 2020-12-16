using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI profileName;
    [SerializeField] private TextMeshProUGUI profileDescription;
    [SerializeField] private Profile[] profiles;
    [SerializeField] private Profile selectedProfile;

    public void StartNewGame()
    {
        GameManager.profileToLoad = selectedProfile;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        print("LoadScene Called");
        StartCoroutine(LoadScene());
    }

    public void ShuffleProfile()
    {
        profiles = Resources.LoadAll<Profile>("Profiles");
        if (selectedProfile != null)
        {
            profiles = profiles.Where(x => x.profileName != selectedProfile.profileName).ToArray();
        }
        selectedProfile = profiles[Random.Range(0, profiles.Length)];
        profileImage.sprite = selectedProfile.profileIcon;
        profileName.text = selectedProfile.profileName;
        profileDescription.text = selectedProfile.description;
    }

    private IEnumerator LoadScene()
    {
        
        GameManager.isGameLoad = true;
        // Start loading the scene
        var asyncLoadLevel = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        // Wait until the level finish loading
        while (!asyncLoadLevel.isDone)
            yield return null;
        // Wait a frame so every Awake and Start method is called

        yield return new WaitForEndOfFrame();
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene(4);
    }
}