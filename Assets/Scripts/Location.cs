using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Location : MonoBehaviour
{
    [SerializeField] private LocationInformation locationInformation;

    // Start is called before the first frame update
    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { DisplayInformation.infoDisplayHelper.DisplayLocationPopup(locationInformation); });
    }
}