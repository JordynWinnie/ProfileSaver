using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScreenScript : MonoBehaviour
{
    public void ContinueGame()
    {
        GameManager.instance.Money += GameManager.instance.currentProfile.income;
        AlertDialog.instance.ShowAlert($"Monthly Income: +${GameManager.instance.currentProfile.income}", AlertDialog.AlertLength.Length_Normal, AlertDialog.AlertType.Message);
        SceneManager.LoadScene(0);
    }
}