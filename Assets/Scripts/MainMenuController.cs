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

    public void StartNewGame()
    {
        GameManager.instance.SetUpValues();
        SceneManager.LoadScene(0);
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
        var profile = GameManager.instance.ReturnRandomProfile();
        GameManager.instance.currentProfile = profile;
        profileImage.sprite = profile.profileIcon;
        profileName.text = profile.name;
        profileDescription.text = $"{profile.description}\nIncome: {profile.income}";
    }
    private IEnumerator LoadScene()
    {
        GameManager.isGameLoad = true;
        // Start loading the scene
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        // Wait until the level finish loading
        while (!asyncLoadLevel.isDone)
            yield return null;
        // Wait a frame so every Awake and Start method is called
        
        yield return new WaitForEndOfFrame();
    }
}