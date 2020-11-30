using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarDisplay : MonoBehaviour
{
    [SerializeField] private Sprite filledStar;
    [SerializeField] private Sprite unfilledStar;

    public void DisplayStar(int numberToDisplay)
    {
        var stars = GetComponentsInChildren<Image>();

        for (int i = 0; i < numberToDisplay; i++)
        {
            stars[i].sprite = filledStar;
        }
    }
}