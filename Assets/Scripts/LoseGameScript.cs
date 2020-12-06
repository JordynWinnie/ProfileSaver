using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseGameScript : MonoBehaviour
{
     public void BackToMainMenu()
     {
          SceneManager.LoadScene(0);
     }
}
