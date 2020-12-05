using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScreenScript : MonoBehaviour
{
    public void ContinueGame()
    {
        GameManager.isGameLoad = true;
        GameManager.isContinueMonth = true;
        SceneManager.LoadScene(1);
    }

    public void TryAnother()
    {
        SceneManager.LoadScene(0);
    }
}