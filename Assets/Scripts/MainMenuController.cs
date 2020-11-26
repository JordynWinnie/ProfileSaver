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

    public void ShuffleProfile()
    {
        var profile = GameManager.instance.ReturnRandomProfile();
        GameManager.instance.currentProfile = profile;
        profileImage.sprite = profile.profileIcon;
        profileName.text = profile.name;
        profileDescription.text = $"{profile.description}\nIncome: {profile.income}";
    }
}