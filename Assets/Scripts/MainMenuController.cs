using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI profileName;
    [SerializeField] private TextMeshProUGUI profileDescription;

    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShuffleProfile()
    {
        var profile = GameManager.instance.ReturnRandomProfile();

        profileImage.sprite = profile.profileIcon;
        profileName.text = profile.name;
        profileDescription.text = $"{profile.description}\nIncome: {profile.income}";
    }
}