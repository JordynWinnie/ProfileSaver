using System.Collections;
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
    [SerializeField] private Profile selectedProfile = null;
 
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
        StartCoroutine(LoadScene());
    }

    public void ShuffleProfile()
    {
        profiles = Resources.LoadAll<Profile>("Profiles");
        selectedProfile = profiles[Random.Range(0, profiles.Length)];
        profileImage.sprite = selectedProfile.profileIcon;
        profileName.text = selectedProfile.profileName;
        profileDescription.text = selectedProfile.description;
    }
    private IEnumerator LoadScene()
    {
        GameManager.isGameLoad = true;
        // Start loading the scene
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(1,LoadSceneMode.Single);
        // Wait until the level finish loading
        while (!asyncLoadLevel.isDone)
            yield return null;
        // Wait a frame so every Awake and Start method is called
        
        yield return new WaitForEndOfFrame();
    }
}